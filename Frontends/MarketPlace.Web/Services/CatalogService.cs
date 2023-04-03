using MarketPlace.Shared.Dtos;
using MarketPlace.Web.Helpers;
using MarketPlace.Web.Services.Interfaces;
using MarketPlace.Web.ViewModels.Catalog;

namespace MarketPlace.Web.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _httpClient;
    private readonly IPhotoStockService _photoStockService;
    private readonly PhotoHelper _photoHelper;

    public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService, PhotoHelper photoHelper)
    {
        _httpClient = httpClient;
        _photoStockService = photoStockService;
        _photoHelper = photoHelper;
    }

    public async Task<bool> CreateProductAsync(ProductCreateInput productCreateInput)
    {
        var resultPhotoService = await _photoStockService.UploadPhoto(productCreateInput.PhotoFormFile);

        if (resultPhotoService != null)
        {
            productCreateInput.Picture = resultPhotoService.Url;
        }
        var response = await _httpClient.PostAsJsonAsync<ProductCreateInput>("products", productCreateInput);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProductAsync(string productId)
    {
        var response = await _httpClient.DeleteAsync("products/" + productId);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<CategoryViewModel>> GetAllCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("categories");
        if (!response.IsSuccessStatusCode)
            return null;
        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();

        return responseSuccess.Data;

    }

    public async Task<List<ProductViewModel>> GetAllProductByUserIdAsync(string userId)
    {
        var response = await _httpClient.GetAsync("products/GetAllByUserId/" + userId);
        if (!response.IsSuccessStatusCode)
            return null;
        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<ProductViewModel>>>();
        responseSuccess.Data.ForEach(x =>
        {
            x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
        });
        return responseSuccess.Data;
    }

    public async Task<List<ProductViewModel>> GetAllProductsAsync()
    {
        var response = await _httpClient.GetAsync("products");
        if (!response.IsSuccessStatusCode)
            return null;
        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<ProductViewModel>>>();
        responseSuccess.Data.ForEach(x =>
        {
            x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
        });
        return responseSuccess.Data;
    }

    public async Task<ProductViewModel> GetByProductIdAsync(string productId)
    {
        var response = await _httpClient.GetAsync("products/" + productId);
        if (!response.IsSuccessStatusCode)
            return null;
        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<ProductViewModel>>();
        responseSuccess.Data.StockPictureUrl = _photoHelper.GetPhotoStockUrl(responseSuccess.Data.Picture);
        return responseSuccess.Data;
    }

    public async Task<bool> UpdateProductAsync(ProductUpdateInput productUpdateInput)
    {
        var resultPhotoService = await _photoStockService.UploadPhoto(productUpdateInput.PhotoFormFile);

        if (resultPhotoService != null)
        {
            _photoStockService.DeletePhoto(productUpdateInput.Picture);
            productUpdateInput.Picture = resultPhotoService.Url;
        }
        var response = await _httpClient.PutAsJsonAsync<ProductUpdateInput>("products", productUpdateInput);
        return response.IsSuccessStatusCode;
    }
}
