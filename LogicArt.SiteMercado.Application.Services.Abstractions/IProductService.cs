using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogicArt.SiteMercado.Domain.Entities;

namespace LogicArt.SiteMercado.Core.Services.Abstractions
{
    public interface IProductService : IService<Product, Guid>
    {

        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);

    }
}
