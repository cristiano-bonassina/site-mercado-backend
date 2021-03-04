using System;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using LogicArt.SiteMercado.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LogicArt.SiteMercado.Presentation.Auth
{
    public class SiteMercadoGrantValidator : IExtensionGrantValidator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SiteMercadoAuthService _siteMercadoAuthService;
        private readonly UserManager<User> _userManager;

        public SiteMercadoGrantValidator(IServiceProvider serviceProvider, SiteMercadoAuthService siteMercadoAuthService, UserManager<User> userManager)
        {
            _serviceProvider = serviceProvider;
            _siteMercadoAuthService = siteMercadoAuthService;
            _userManager = userManager;
        }

        public string GrantType { get; } = "sitemercado-password";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var request = context.Request;
            var userName = request.Raw.Get("username");
            if (string.IsNullOrWhiteSpace(userName))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "Invalid username");
                return;
            }

            var password = request.Raw.Get("password");
            if (string.IsNullOrWhiteSpace(password))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "Invalid password");
                return;
            }

            var siteMercadoAuthResponse = await _siteMercadoAuthService.AuthenticateAsync(userName, password);
            if (!siteMercadoAuthResponse.Success)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, siteMercadoAuthResponse.Error);
                return;
            }

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new User {UserName = userName};
                var createUserResult = await _userManager.CreateAsync(user, password);
                if (!createUserResult.Succeeded)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "Could not create the user");
                    return;
                }
            }

            var dbContext = _serviceProvider.GetRequiredService<DbContext>();
            await dbContext.SaveChangesAsync();

            context.Result = new GrantValidationResult(user.Id.ToString("N"), this.GrantType);
        }
    }
}
