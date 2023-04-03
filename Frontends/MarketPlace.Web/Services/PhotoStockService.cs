using MarketPlace.Shared.Dtos;
using MarketPlace.Web.Services.Interfaces;
using MarketPlace.Web.ViewModels.PhotoStocks;

namespace MarketPlace.Web.Services;

public class PhotoStockService : IPhotoStockService
{
    private readonly HttpClient _httpClient;

    public PhotoStockService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> DeletePhoto(string photoUrl)
    {
        var response = await _httpClient.DeleteAsync($"photo?photoUrl={photoUrl}");
        return response.IsSuccessStatusCode;
    }

    public async Task<PhotoViewModel> UploadPhoto(IFormFile photo)
    {
        if (photo is null || photo.Length <= 0)
            return null;
        var randomFilename = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";

        using var ms = new MemoryStream();
        await photo.CopyToAsync(ms);

        var multipartContent = new MultipartFormDataContent();

        multipartContent.Add(new ByteArrayContent(ms.ToArray()), "photo", randomFilename);

        var response = await _httpClient.PostAsync("photo", multipartContent);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var responseImage = await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();
        return responseImage.Data;
    }
}
