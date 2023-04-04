using MarketPlace.Web.Services.Interfaces;
using MarketPlace.Web.ViewModels.Basket;
using MarketPlace.Web.ViewModels.Discounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public BasketController(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _basketService.Get());
        }
        public async Task<IActionResult> AddBasketItem(string id)
        {
            var product = await _catalogService.GetByProductIdAsync(id);
            var basketItem = new BasketItemViewModel { ProductId = product.Id, ProductName = product.Name, Price = product.Price, Picture = product.MainPhotoStockUrl };
            await _basketService.AddBasketItem(basketItem);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> RemoveBasketItem(string productId)
        {
            await _basketService.RemoveBasketItem(productId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ApplyDiscount(DiscountApplyInput discountApplyInput)
        {
            if (!ModelState.IsValid)
            {
                TempData["discountError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).First();
                return RedirectToAction(nameof(Index));
            }

            var discountStatus = await _basketService.ApplyDiscount(discountApplyInput.Code);

            TempData["discountStatus"] = discountStatus;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> CancelApplyDiscount()
        {
            await _basketService.CancelApplyDiscount();

            return RedirectToAction(nameof(Index));
        }
    }
}
