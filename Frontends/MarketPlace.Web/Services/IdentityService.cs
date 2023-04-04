using IdentityModel.Client;
using MarketPlace.Shared.Dtos;
using MarketPlace.Web.Models;
using MarketPlace.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace MarketPlace.Web.Services;

public class IdentityService : IIdentitiyService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ClientSettings _clientSettings;
    private readonly ServiceAPISettings _serviceAPISettings;

    public IdentityService(HttpClient httpClient, IHttpContextAccessor contextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceAPISettings> serviceAPISettings)
    {
        _httpClient = httpClient;
        _contextAccessor = contextAccessor;
        _clientSettings = clientSettings.Value;
        _serviceAPISettings = serviceAPISettings.Value;
    }

    public async Task<TokenResponse> GetAccessTokenByRefreshToken()
    {
        var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
        {
            Address = _serviceAPISettings.IdentityBaseUri,
            Policy = new DiscoveryPolicy { RequireHttps = false }
        });

        if (disco.IsError)
        {
            throw disco.Exception;
        }

        var refreshToken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

        RefreshTokenRequest refreshTokenRequest = new()
        {
            ClientId = _clientSettings.WebClientForUser.ClientId,
            ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
            RefreshToken = refreshToken,
            Address = disco.TokenEndpoint
        };

        var token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

        if (token.IsError)
        {
            return null;
        }

        var authenticationTokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                   new AuthenticationToken{ Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},

                      new AuthenticationToken{ Name=OpenIdConnectParameterNames.ExpiresIn,Value= DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            };

        var authenticationResult = await _contextAccessor.HttpContext.AuthenticateAsync();

        var properties = authenticationResult.Properties;
        properties.StoreTokens(authenticationTokens);

        await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);

        return token;
    }

    public async Task RevokeRefreshToken()
    {
        var discount = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
        {
            Address = _serviceAPISettings.IdentityBaseUri,
        });
        if (discount.IsError)
        {
            throw discount.Exception;
        }
        var refreshToken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

        TokenRevocationRequest tokenRevocationRequest = new()
        {
            ClientId = _clientSettings.WebClientForUser.ClientId,
            ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
            Address = discount.RevocationEndpoint,
            Token = refreshToken,
            TokenTypeHint = "refresh_token"
        };

        await _httpClient.RevokeTokenAsync(tokenRevocationRequest);
    }

    public async Task<Response<bool>> SignIn(SignInInput signInInput)
    {
        var discount = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
        {
            Address = _serviceAPISettings.IdentityBaseUri,
        });
        if (discount.IsError)
        {
            throw discount.Exception;
        }

        var passwordTokenRequest = new PasswordTokenRequest()
        {
            ClientId = _clientSettings.WebClientForUser.ClientId,
            ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
            Password = signInInput.Password,
            UserName = signInInput.Email,
            Address = discount.TokenEndpoint
        };

        var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);
        if (token.IsError)
        {
            var responseCode = await token.HttpResponse.Content.ReadAsStringAsync();
            var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseCode, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return Response<bool>.Failed(errorDto.Errors, 404);
        }

        var userInfoRequest = new UserInfoRequest
        {
            Token = token.AccessToken,
            Address = discount.UserInfoEndpoint
        };

        var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequest);

        if (userInfo.IsError)
        {
            throw userInfo.Exception;
        }

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var authenticationProperties = new AuthenticationProperties();

        authenticationProperties.StoreTokens(new List<AuthenticationToken>() {
           new AuthenticationToken()
           {
               Name = OpenIdConnectParameterNames.AccessToken,
               Value = token.AccessToken
           },
           new AuthenticationToken()
           {
               Name = OpenIdConnectParameterNames.RefreshToken,
               Value = token.RefreshToken
           },
            new AuthenticationToken()
           {
               Name = OpenIdConnectParameterNames.ExpiresIn,
               Value = DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)
           }

        });

        authenticationProperties.IsPersistent = signInInput.IsRemember;

        await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

        return Response<bool>.Success(200);
    }
    public async Task<Response<bool>> SignUp(SignUpInput signUpInput)
    {
        var response = await _httpClient.PostAsJsonAsync<SignUpInput>("signup", signUpInput);

        return Response<bool>.Success(200);

    }
}
