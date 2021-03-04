using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogicArt.Arch.Application.Repositories.Abstractions;
using LogicArt.SiteMercado.Core.Services.Abstractions;
using LogicArt.SiteMercado.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogicArt.SiteMercado.Application.Services
{
    public class ProductService : Service<Product, Guid>, IProductService
    {
        public ProductService(IRepository<Product, Guid> repository) : base(repository)
        {
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await this.Repository
                .Query()
                .OrderBy(x => x.Name)
                .ToListAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            var products = await this.Repository
                .Query()
                .Where(x => x.Name.Contains(name))
                .OrderBy(x => x.Name)
                .ToListAsync();
            return products;
        }
    }
}
