using MarketPlace.Basket.Dtos;
using MarketPlace.Shared.Dtos;

namespace MarketPlace.Basket.Services;

public interface IBasketService
{
    Task<Response<BasketDto>> GetBasket(string userId);
    Task<Response<bool>> SaveOrUpdate(BasketDto basketDto);
    Task<Response<bool>> Delete(string userId);
}
