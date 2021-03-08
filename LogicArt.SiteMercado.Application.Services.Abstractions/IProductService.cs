using System;
using System.Collections.Generic;
using LogicArt.SiteMercado.Domain.Entities;

namespace LogicArt.SiteMercado.Core.Services.Abstractions
{
    public interface IProductService : IService<Product, Guid>
    {
        IAsyncEnumerable<Product> GetProductsByNameAsync(string name);
    }
}
