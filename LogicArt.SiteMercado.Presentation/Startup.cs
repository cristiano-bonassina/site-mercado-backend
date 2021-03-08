using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.AspNetIdentity;
using LogicArt.Arch.Application.Bus;
using LogicArt.Arch.Application.Bus.Abstractions;
using LogicArt.Arch.Application.Repositories.Abstractions;
using LogicArt.Arch.Application.Repositories.Abstractions.Transaction;
using LogicArt.Arch.Application.Validations.Abstractions;
using LogicArt.Arch.Infrastructure.Persistence.EntityFramework;
using LogicArt.Identity.Extensions;
using LogicArt.Identity.Storage;
using LogicArt.SiteMercado.Application;
using LogicArt.SiteMercado.Application.Adapters;
using LogicArt.SiteMercado.Application.Adapters.Abstractions;
using LogicArt.SiteMercado.Application.Data;
using LogicArt.SiteMercado.Application.Events;
using LogicArt.SiteMercado.Application.Services;
using LogicArt.SiteMercado.Application.Validations;
using LogicArt.SiteMercado.Core.Services.Abstractions;
using LogicArt.SiteMercado.Domain.Entities;
using LogicArt.SiteMercado.Infrastructure;
using LogicArt.SiteMercado.Infrastructure.Persistence;
using LogicArt.SiteMercado.Infrastructure.Persistence.Repositories;
using LogicArt.SiteMercado.Presentation.Auth;
using LogicArt.SiteMercado.Presentation.Identity;
using LogicArt.SiteMercado.Presentation.Monitoring;
using LogicArt.SiteMercado.Presentation.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace LogicArt.SiteMercado.Presentation
{
    public class Startup
    {
        static Startup()
        {
            // By default, Microsoft has some legacy claim mapping that converts
            // standard JWT claims into proprietary ones. This removes those mappings.

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
        }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Services

            services.AddHostedService<DatabaseMigrationService>();

            #endregion

            #region Health

            services.AddHealthChecks()
                .AddCheck<HealthCheck>("HealthCheck");

            #endregion

            #region CORS

            services.AddCors(options => options.AddPolicy("default", builder => builder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .WithExposedHeaders("X-Total-Count")
                .Build()));

            #endregion

            #region Api Versioning

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(p =>
            {
                p.GroupNameFormat = "'v'VVV";
                p.SubstituteApiVersionInUrl = true;
            });

            #endregion

            #region Swagger

            services.AddSwaggerGen().AddSwaggerGenNewtonsoftSupport();

            #endregion

            #region Database Connection

            var connectionString = System.Environment.GetEnvironmentVariable("DATABASE_URL");
            var dbContextOptions = new DbContextOptionsBuilder()
                .EnableDetailedErrors(!this.Environment.IsProduction())
                .EnableSensitiveDataLogging(!this.Environment.IsProduction())
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString)
                .Options;
            services.AddSingleton(dbContextOptions);

            #endregion

            #region Identity

            services.ConfigureIdentity<User>();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            });

            var authKeyPath = Path.Combine(AppContext.BaseDirectory, "Resources", "auth-key");
            var authCertificate = new X509Certificate2(authKeyPath, "0000");
            var securityKey = new X509SecurityKey(authCertificate);
            var signingCredential = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

            services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
                .AddInMemoryApiResources(IdentityConfiguration.ApiResources)
                .AddInMemoryClients(IdentityConfiguration.Clients)
                .AddSigningCredential(signingCredential)
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator<User>>()
                .AddPersistedGrantStore<PersistedGrantStore>()
                .AddExtensionGrantValidator<SiteMercadoGrantValidator>()
                .AddProfileService<ProfileService>();

            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    var documentPath = Path.Combine(AppContext.BaseDirectory, "Resources", "open-id");
                    var documentMetadata = File.ReadAllText(documentPath);
                    var configuration = new OpenIdConnectConfiguration(documentMetadata);

                    options.Audience = "api";
                    options.Authority = "http://localhost";
                    options.Configuration = configuration;
                    options.RequireHttpsMetadata = this.Environment.IsProduction();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = securityKey,
                        NameClaimType = "name",
                        RoleClaimType = "role",
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });

            #endregion

            services.AddControllers();

            services.AddSingleton<IImageProcessor, ImageProcessor>();
            services.AddSingleton<IEventSchemaProvider, EventSchemaProvider>();
            services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IEventSchemaProvider>().GetSchema());
            services.AddScoped<IEventProvider, EventProvider>();
            services.AddScoped<IEventBus, EventBus>();

            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<DbContext>(serviceProvider => serviceProvider.GetService<ApplicationDbContext>());
            services.AddScoped<ArchDbContext>(serviceProvider => serviceProvider.GetService<ApplicationDbContext>());

            services.AddScoped<IUnitOfWork, EntityFrameworkUnitOfWork>();

            services.AddScoped<IAdapter<Product, ProductDTO>, ProductAdapter>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRepository<Product, Guid>, ProductRepository>();
            services.AddScoped<IRepository<User, Guid>, UserRepository>();
            services.AddScoped<IValidation<Product>, ProductValidation>();
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseHealthChecks("/health");
            app.UseApiVersioning();
            app.UseSwagger()
                .UseSwaggerUI(options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                    options.DocExpansion(DocExpansion.List);
                });
            app.UseCors("default");
            app.UseIdentityServer();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
