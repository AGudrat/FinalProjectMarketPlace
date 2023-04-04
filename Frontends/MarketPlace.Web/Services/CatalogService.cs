using MarketPlace.Shared.Dtos;
using MarketPlace.Web.Helpers;
using MarketPlace.Web.Services.Interfaces;
using MarketPlace.Web.ViewModels.Catalog;
using Nest;

namespace MarketPlace.Web.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _httpClient;
    private readonly IPhotoStockService _photoStockService;
    private readonly PhotoHelper _photoHelper;
    private readonly IElasticClient _elasticClient;
    public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService, PhotoHelper photoHelper, IElasticClient elasticClient)
    {
        _httpClient = httpClient;
        _photoStockService = photoStockService;
        _photoHelper = photoHelper;
        _elasticClient = elasticClient;
    }

    public async Task<bool> CreateCategoryAsync(CategoryCreateInput categoryCreateInput)
    {
        var photoUrl = await _photoStockService.UploadPhoto(categoryCreateInput.Photo);
        if (photoUrl != null)
        {
            categoryCreateInput.PhotoUrl = photoUrl.Url;
        }
        var response = await _httpClient.PostAsJsonAsync<CategoryCreateInput>("categories", categoryCreateInput);
        return response.IsSuccessStatusCode;
    }
    public async Task<CategoryViewModel> GetByIdCategory(string id)
    {
        var response = await _httpClient.GetAsync("categories/" + id);

        if (!response.IsSuccessStatusCode)
            return null;
        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<CategoryViewModel>>();
        responseSuccess.Data.PhotoStockUrl = _photoHelper.GetPhotoStockUrl(responseSuccess.Data.PhotoStockUrl);
        return responseSuccess.Data;

    }

    public async Task<bool> UpdateCategoryAsync(CategoryUpdateInput categoryUpdateInput)
    {
        var resultPhotoService = await _photoStockService.UploadPhoto(categoryUpdateInput.Photo);

        if (resultPhotoService != null)
        {
            _photoStockService.DeletePhoto(categoryUpdateInput.PhotoUrl);
            categoryUpdateInput.PhotoUrl = resultPhotoService.Url;
        }
        var response = await _httpClient.PutAsJsonAsync<CategoryUpdateInput>("categories", categoryUpdateInput);
        return response.IsSuccessStatusCode;
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
        var response = await _httpClient.DeleteAsync("categories/" + productId);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<CategoryViewModel>> GetAllCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("categories");
        if (!response.IsSuccessStatusCode)
            return null;
        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();
        responseSuccess.Data.ForEach(x =>
        {
            x.PhotoStockUrl = _photoHelper.GetPhotoStockUrl(x.PhotoUrl);
        });
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
        List<string> otherPhotos = new();
        if (productUpdateInput.OtherPhotos is not null)
        {
            foreach (var item in productUpdateInput.OtherPhotosUrl)
            {
                _photoStockService.DeletePhoto(item);
            }
            int count = 0;
            foreach (var item in productUpdateInput.OtherPhotos)
            {
                if (count > 3) { break; }
                var resultOtherPhotoService = await _photoStockService.UploadPhoto(item);

                if (resultOtherPhotoService != null)
                {

                    otherPhotos.Add(resultOtherPhotoService.Url);
                }
            }


            productUpdateInput.OtherPhotosUrl = otherPhotos;
        }
        var response = await _httpClient.PutAsJsonAsync<ProductUpdateInput>("products", productUpdateInput);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCategoryAsync(string id)
    {
        var response = await _httpClient.DeleteAsync("categories" + id);
        return response.IsSuccessStatusCode;
    }
}
