using System;
using LogicArt.SiteMercado.Application.Data;
using LogicArt.SiteMercado.Core.Services.Abstractions;
using LogicArt.SiteMercado.Domain.Entities;

namespace LogicArt.SiteMercado.Application.Adapters
{
    public class ProductAdapter : Adapter<Product, ProductDTO, Guid>
    {
        public ProductAdapter(IService<Product, Guid> service) : base(service)
        {
        }

        public override void WriteEntity(ProductDTO resource, Product entity)
        {
            entity.Image = resource.Image;
            entity.Name = resource.Name;
            entity.Price = resource.Price;
        }

        public override void WriteResource(Product entity, ProductDTO resource)
        {
            resource.Image = entity.Image;
            resource.Name = entity.Name;
            resource.Price = entity.Price;
        }
    }
}
