using FluentValidation;
using MarketPlace.Web.ViewModels.Discounts;

namespace MarketPlace.Web.Validators;

public class DiscountApplyInputValidator : AbstractValidator<DiscountApplyInput>
{
    public DiscountApplyInputValidator()
    {
        RuleFor(x => x.Code).NotEmpty().WithMessage("This field can not be empty.");
    }
}
