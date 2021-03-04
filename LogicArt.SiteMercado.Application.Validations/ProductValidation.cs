using FluentValidation;
using LogicArt.Arch.Application.Validations;
using LogicArt.SiteMercado.Domain.Entities;

namespace LogicArt.SiteMercado.Application.Validations
{
    public class ProductValidation : Validation<Product>
    {
        public ProductValidation()
        {
            this.RuleFor(x => x.Image).NotEmpty();
            this.RuleFor(x => x.Name).NotEmpty();
        }
    }
}
