using System;
using System.Collections.Generic;
using System.Linq;
using LogicArt.Arch.Application.Repositories.Abstractions;
using LogicArt.SiteMercado.Core.Services.Abstractions;
using LogicArt.SiteMercado.Domain.Entities;

namespace LogicArt.SiteMercado.Application.Services
{
    public class ProductService : Service<Product, Guid>, IProductService
    {
        public ProductService(IRepository<Product, Guid> repository) : base(repository)
        {
        }

        public override IAsyncEnumerable<Product> GetAllAsync() => this.Repository
            .Query()
            .OrderBy(x => x.Name)
            .ToAsyncEnumerable();

        public IAsyncEnumerable<Product> GetProductsByNameAsync(string name) => this.Repository
                .Query()
                .Where(x => x.Name.Contains(name))
                .OrderBy(x => x.Name)
                .ToAsyncEnumerable();
    }
}
