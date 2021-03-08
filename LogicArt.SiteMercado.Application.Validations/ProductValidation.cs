using FluentValidation;
using LogicArt.Arch.Application.Validations;
using LogicArt.SiteMercado.Domain.Entities;

namespace LogicArt.SiteMercado.Application.Validations
{
    public class ProductValidation : Validation<Product>
    {
        public ProductValidation()
        {
            this.RuleFor(x => x.Image)
                .NotEmpty()
                .WithMessage("A imagem do produto é obrigatória");
            this.RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome do produto é obrigatório");
        }
    }
}
