using MarketPlace.Web.Exceptions;
using MarketPlace.Web.Models;
using MarketPlace.Web.Services.Interfaces;
using MarketPlace.Web.ViewModels.Catalog;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Diagnostics;

namespace MarketPlace.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IElasticClient _elasticClient;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<HomeController> _logger;
        private readonly ICatalogService _catalogService;
        public HomeController(ILogger<HomeController> logger, ICatalogService catalogService, IElasticClient elasticClient, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _catalogService = catalogService;
            _elasticClient = elasticClient;
            _hostingEnvironment = hostingEnvironment;
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

        [HttpGet]
        public async Task<IActionResult> Search(string productName)
        {
            await Import();
            var articleList = new List<ProductViewModel>();
            if (!string.IsNullOrEmpty(productName))
            {
                articleList = GetSearch(productName).ToList();
            }
            var categoriesResponse = await _catalogService.GetAllCategoriesAsync();
            return View((articleList, categoriesResponse));
        }
        public async Task Import()
        {
            var products = await _catalogService.GetAllProductsAsync();
            foreach (var article in products)
            {
                _elasticClient.IndexDocumentAsync(article);
            }
        }
        public IList<ProductViewModel> GetSearch(string keyword)
        {

            var result = _elasticClient.SearchAsync<ProductViewModel>(
                s => s.Query(
                    q => q.QueryString(
                        d => d.Query('*' + keyword + '*')
                    )).Size(5000));

            var finalResult = result;
            var finalContent = finalResult.Result.Documents.ToList();
            return finalContent;
        }

    }
}