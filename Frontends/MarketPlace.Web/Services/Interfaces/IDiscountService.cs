using MarketPlace.Web.ViewModels.Discounts;

namespace MarketPlace.Web.Services.Interfaces;

public interface IDiscountService
{
    Task<DiscountViewModel> GetDiscount(string discountCode);
}
