using MarketPlace.Basket.Dtos;
using MarketPlace.Basket.Services;
using MarketPlace.Shared.ControllerBases;
using MarketPlace.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : CustomBaseController
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BasketController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            return CreateActionResult(await _basketService.GetBasket(_sharedIdentityService.GetUserId));
        }
        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket(BasketDto basketDto)
        {
            basketDto.UserId = _sharedIdentityService.GetUserId;
            var response = await _basketService.SaveOrUpdate(basketDto);
            return CreateActionResult(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            return CreateActionResult(await _basketService.Delete(_sharedIdentityService.GetUserId));
        }
    }
}
