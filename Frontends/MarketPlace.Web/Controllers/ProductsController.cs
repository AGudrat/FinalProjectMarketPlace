using MarketPlace.Shared.Services;
using MarketPlace.Web.Services.Interfaces;
using MarketPlace.Web.ViewModels.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarketPlace.Web.Controllers;

[Authorize]
public class ProductsController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly ISharedIdentityService _sharedIdentityService;
    public ProductsController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
    {
        _catalogService = catalogService;
        _sharedIdentityService = sharedIdentityService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _catalogService.GetAllProductByUserIdAsync(_sharedIdentityService.GetUserId));
    }

    public async Task<IActionResult> Create()
    {
        var categories = await _catalogService.GetAllCategoriesAsync();
        ViewBag.categoryList = new SelectList(categories, "Id", "Name");
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateInput productCreateInput)
    {
        var categories = await _catalogService.GetAllCategoriesAsync();
        ViewBag.categoryList = new SelectList(categories, "Id", "Name");
        if (!ModelState.IsValid)
            return View();

        productCreateInput.UserId = _sharedIdentityService.GetUserId;

        await _catalogService.CreateProductAsync(productCreateInput);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Update(string id)
    {
        var product = await _catalogService.GetByProductIdAsync(id);
        var categories = await _catalogService.GetAllCategoriesAsync();
        ViewBag.categoryList = new SelectList(categories, "Id", "Name", product.Id);
        if (product.Id is null)
        {
            return RedirectToAction(nameof(Index));
        }
        ProductUpdateInput productUpdateInput = new()
        {
            Id = product.Id,
            Name = product.Name,
            CategoryId = product.CategoryId,
            Description = product.Description,
            MainPhotoUrl = product.MainPhotoUrl,
            OtherPhotosUrl = product.OtherPhotosUrl,
            Price = product.Price,
            UserId = product.UserId,
        };
        return View(productUpdateInput);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ProductUpdateInput productUpdateInput)
    {
        var categories = await _catalogService.GetAllCategoriesAsync();
        ViewBag.categoryList = new SelectList(categories, "Id", "Name", productUpdateInput.Id);
        if (!ModelState.IsValid)
            return View();
        await _catalogService.UpdateProductAsync(productUpdateInput);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(string id)
    {
        await _catalogService.DeleteProductAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
