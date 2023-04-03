using MarketPlace.Web.Services.Interfaces;
using MarketPlace.Web.ViewModels;

namespace MarketPlace.Web.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserViewModel> GetUser()
    {
        return await _httpClient.GetFromJsonAsync<UserViewModel>("api/user/getuser");
    }
}
