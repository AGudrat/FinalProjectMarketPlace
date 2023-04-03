using MarketPlace.Web.ViewModels;

namespace MarketPlace.Web.Services.Interfaces;

public interface IUserService
{
    Task<UserViewModel> GetUser();
}
