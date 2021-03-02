using LogicArt.Arch.Application.Bus.Abstractions;
using LogicArt.Arch.Infrastructure.Persistence.EntityFramework;
using LogicArt.SiteMercado.Infrastructure.Persistence.Mapping;
using LogicArt.Identity.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LogicArt.SiteMercado.Infrastructure.Persistence
{
    public class ApplicationDbContext : ArchDbContext
    {

        public ApplicationDbContext(DbContextOptions options, IEventBus bus) : base(options, bus)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyIdentityConfiguration()
                .ApplyConfiguration(new ProductTypeConfiguration())
                .ApplyConfiguration(new UserTypeConfiguration());
            base.OnModelCreating(builder);
        }

    }
}
