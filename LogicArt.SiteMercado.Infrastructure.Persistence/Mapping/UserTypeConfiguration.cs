using System;
using LogicArt.Arch.Infrastructure.Persistence.EntityFramework.Mapping;
using LogicArt.SiteMercado.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogicArt.SiteMercado.Infrastructure.Persistence.Mapping
{
    public class UserTypeConfiguration : EntityTypeConfiguration<User, Guid>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.HasAlternateKey(x => x.UserName);
        }
    }
}
