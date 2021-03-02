using System;
using LogicArt.Arch.Domain.Entities;

namespace LogicArt.SiteMercado.Domain.Entities
{
    public class Product : Entity<Guid>
    {

        private byte[] _image;
        private string _name;
        private decimal _price;

        public byte[] Image
        {
            get => _image;
            set => this.SetWithNotify(value, ref _image);
        }

        public string Name
        {
            get => _name;
            set => this.SetWithNotify(value, ref _name);
        }

        public decimal Price
        {
            get => _price;
            set => this.SetWithNotify(value, ref _price);
        }

        public override string ToString() => this.Name;

    }
}
