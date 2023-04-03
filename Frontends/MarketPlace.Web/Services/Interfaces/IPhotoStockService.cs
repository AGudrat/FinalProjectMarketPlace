using MarketPlace.Web.ViewModels.PhotoStocks;

namespace MarketPlace.Web.Services.Interfaces;

public interface IPhotoStockService
{
    Task<PhotoViewModel> UploadPhoto(IFormFile photo);
    Task<bool> DeletePhoto(string photoUrl);


}
