using FluentValidation;
using MarketPlace.Web.ViewModels.Catalog;

namespace MarketPlace.Web.Validators;

public class ProductCreateInputValidator : AbstractValidator<ProductCreateInput>
{
    public ProductCreateInputValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name can not be empty");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description can not be empty");
        RuleFor(x => x.Price).NotEmpty().WithMessage("Price can not be empty").ScalePrecision(2, 6).WithMessage("Price format not valid");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Select category");
    }
}
