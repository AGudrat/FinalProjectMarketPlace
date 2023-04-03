using MarketPlace.Shared.Dtos;
using MarketPlace.Shared.Services;
using MarketPlace.Web.Services.Interfaces;
using MarketPlace.Web.ViewModels.Basket;

namespace MarketPlace.Web.Services;

public class BasketService : IBasketService
{
    private readonly HttpClient _httpClient;
    private readonly IDiscountService _discountService;
    private readonly ISharedIdentityService _sharedIdentityService;
    public BasketService(HttpClient httpClient, IDiscountService discountService, ISharedIdentityService sharedIdentityService)
    {
        _httpClient = httpClient;
        _discountService = discountService;
        _sharedIdentityService = sharedIdentityService;
    }

    public async Task AddBasketItem(BasketItemViewModel basketItemViewModel)
    {
        var basket = await Get();

        if (basket != null)
        {
            var basketItem = basket.BasketItems.FirstOrDefault(x => x.ProductId == basketItemViewModel.ProductId);
            if (basketItem is null)
            {
                basket.BasketItems.Add(basketItemViewModel);
            }
            else
            {
                basketItem.Quantity += 1;
            }
        }
        else
        {
            basket = new BasketViewModel();
            basket.BasketItems.Add(basketItemViewModel);
        }
        await SaveOrUpdate(basket);
    }

    public async Task<bool> ApplyDiscount(string discountCode)
    {
        await CancelApplyDiscount();

        var basket = await Get();
        if (basket is null)
            return false;

        var hasDiscount = await _discountService.GetDiscount(discountCode);

        if (hasDiscount is null)
            return false;

        basket.ApplyDiscount(hasDiscount.Code, hasDiscount.Rate);

        return await SaveOrUpdate(basket);
    }

    public async Task<bool> CancelApplyDiscount()
    {
        var basket = await Get();
        if (basket is null || basket.DiscountCode is null)
            return false;

        basket.CancelDiscount();

        return await SaveOrUpdate(basket);
    }

    public async Task<bool> Delete()
    {
        var result = await _httpClient.DeleteAsync("basket");
        return result.IsSuccessStatusCode;
    }

    public async Task<BasketViewModel> Get()
    {
        var response = await _httpClient.GetAsync("basket");
        if (!response.IsSuccessStatusCode)
            return null;

        var basketViewModel = await response.Content.ReadFromJsonAsync<Response<BasketViewModel>>();
        return basketViewModel.Data;
    }

    public async Task<bool> RemoveBasketItem(string productId)
    {
        var basket = await Get();
        if (basket is null)
            return false;
        var deleteBasketItem = basket.BasketItems.FirstOrDefault(x => x.ProductId == productId);
        if (deleteBasketItem is null)
            return false;
        var deleteResult = basket.BasketItems.Remove(deleteBasketItem);

        if (!deleteResult)
            return false;
        if (!basket.BasketItems.Any())
            basket.DiscountCode = null;
        return await SaveOrUpdate(basket);
    }

    public async Task<bool> SaveOrUpdate(BasketViewModel basketViewModel)
    {
        basketViewModel.UserId = _sharedIdentityService.GetUserId;
        var response = await _httpClient.PostAsJsonAsync<BasketViewModel>("basket", basketViewModel);
        return response.IsSuccessStatusCode;
    }
}
