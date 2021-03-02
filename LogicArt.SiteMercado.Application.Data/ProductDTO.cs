using System;
using LogicArt.Arch.Application.Data;

namespace LogicArt.SiteMercado.Application.Data
{
    public class ProductDTO : Resource<Guid>
    {
        public byte[] Image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
