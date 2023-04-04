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
        var mainPhotoUrl = await _photoStockService.UploadPhoto(productCreateInput.MainPhoto);
        if (mainPhotoUrl != null)
        {
            productCreateInput.MainPhotoUrl = mainPhotoUrl.Url;
        }

        foreach (var item in productCreateInput.OtherPhotos)
        {
            var otherPhotoUrl = await _photoStockService.UploadPhoto(item);
            if (otherPhotoUrl != null)
            {
                productCreateInput.OtherPhotosUrl.Add(otherPhotoUrl.Url);
            }
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
            x.MainPhotoStockUrl = _photoHelper.GetPhotoStockUrl(x.MainPhotoUrl);
        });

        responseSuccess.Data.ForEach(x =>
        {
            foreach (var item in x.OtherPhotosUrl)
            {
                x.OtherPhotosStockUrl.Add(_photoHelper.GetPhotoStockUrl(item));
            }
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
            x.MainPhotoStockUrl = _photoHelper.GetPhotoStockUrl(x.MainPhotoUrl);
        });

        responseSuccess.Data.ForEach(x =>
        {
            foreach (var item in x.OtherPhotosUrl)
            {
                x.OtherPhotosStockUrl.Add(_photoHelper.GetPhotoStockUrl(item));
            }
        });
        return responseSuccess.Data;
    }

    public async Task<ProductViewModel> GetByProductIdAsync(string productId)
    {
        var response = await _httpClient.GetAsync("products/" + productId);
        if (!response.IsSuccessStatusCode)
            return null;
        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<ProductViewModel>>();
        responseSuccess.Data.MainPhotoStockUrl = _photoHelper.GetPhotoStockUrl(responseSuccess.Data.MainPhotoUrl);
        foreach (var item in responseSuccess.Data.OtherPhotosUrl)
        {
            responseSuccess.Data.OtherPhotosStockUrl.Add(_photoHelper.GetPhotoStockUrl(item));
        }
        return responseSuccess.Data;
    }

    public async Task<bool> UpdateProductAsync(ProductUpdateInput productUpdateInput)
    {
        var resultPhotoService = await _photoStockService.UploadPhoto(productUpdateInput.MainPhoto);

        if (resultPhotoService != null)
        {
            _photoStockService.DeletePhoto(productUpdateInput.MainPhotoUrl);
            productUpdateInput.MainPhotoUrl = resultPhotoService.Url;
        }
        foreach (var item in productUpdateInput.OtherPhotos)
        {
            var resultOtherPhotoService = await _photoStockService.UploadPhoto(item);

            if (resultOtherPhotoService != null)
            {
                _photoStockService.DeletePhoto(item.FileName);
                productUpdateInput.OtherPhotosUrl.Add(resultOtherPhotoService.Url);
            }
        }
        var response = await _httpClient.PutAsJsonAsync<ProductUpdateInput>("products", productUpdateInput);
        return response.IsSuccessStatusCode;
    }
}
