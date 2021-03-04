using System;
using LogicArt.Arch.Infrastructure.Persistence.EntityFramework.Mapping;
using LogicArt.SiteMercado.Domain.Entities;

namespace LogicArt.SiteMercado.Infrastructure.Persistence.Mapping
{
    public class ProductTypeConfiguration : EntityTypeConfiguration<Product, Guid>
    {
    }
}
