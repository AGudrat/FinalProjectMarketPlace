using IdentityModel.Client;
using MarketPlace.Shared.Dtos;
using MarketPlace.Web.Models;

namespace MarketPlace.Web.Services.Interfaces
{
    public interface IIdentitiyService
    {
        Task<Response<bool>> SignIn(SignInInput signInInput);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
    }
}
