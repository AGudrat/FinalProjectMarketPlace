using MarketPlace.Web.Exceptions;
using MarketPlace.Web.Models;
using MarketPlace.Web.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MarketPlace.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICatalogService _catalogService;
        public HomeController(ILogger<HomeController> logger, ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index()
        {
            var productResponse = await _catalogService.GetAllProductsAsync();
            var categoriesResponse = await _catalogService.GetAllCategoriesAsync();

            return View((productResponse, categoriesResponse));
        }
        public async Task<IActionResult> Detail(string id)
        {
            var products = await _catalogService.GetAllProductsAsync();
            var selectedProduct = await _catalogService.GetByProductIdAsync(id);
            return View((selectedProduct, products.Where(x => x.Category.Name == selectedProduct.Category.Name && x.Id != id).OrderByDescending(x => x.CreatedTime).Take(6).ToList()));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorFuture = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (errorFuture != null && errorFuture.Error is UnAuthorizeExceptions)
            {
                return RedirectToAction(nameof(AuthController.LogOut), "Auth");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Search()
        {
            return View(await _catalogService.GetAllProductsAsync());
        }

        //[HttpPost]
        //public async Task<IActionResult> Search(string )
        //{

        //}
    }
}