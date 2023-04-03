using MarketPlace.Web.Models;
using Microsoft.Extensions.Options;

namespace MarketPlace.Web.Helpers;

public class PhotoHelper
{
    private readonly ServiceAPISettings _serviceAPISettings;

    public PhotoHelper(IOptions<ServiceAPISettings> serviceAPISettings)
    {
        _serviceAPISettings = serviceAPISettings.Value;
    }

    public string GetPhotoStockUrl(string photoUrl)
    {
        return $"{_serviceAPISettings.PhotoStockUri}/photos/{photoUrl}";
    }
}
