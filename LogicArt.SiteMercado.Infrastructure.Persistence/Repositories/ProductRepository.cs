using System;
using LogicArt.Arch.Infrastructure.Persistence.EntityFramework;
using LogicArt.SiteMercado.Application.Repositories.Abstractions;
using LogicArt.SiteMercado.Domain.Entities;

namespace LogicArt.SiteMercado.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : Repository<Product, Guid>, IProductRepository
    {
        public ProductRepository(ArchDbContext context) : base(context)
        {
        }
    }
}
