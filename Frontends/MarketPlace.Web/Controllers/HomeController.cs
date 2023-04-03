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

            return View(await _catalogService.GetAllProductsAsync());
        }
        public async Task<IActionResult> Detail(string id)
        {
            return View(await _catalogService.GetByProductIdAsync(id));
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

    }
}