using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using MarketPlace.Web.Models;
using MarketPlace.Web.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace MarketPlace.Web.Services;

public class ClientCredentialTokenService : IClientCredentialTokenService
{
    private readonly ServiceAPISettings _serviceAPISettings;
    private readonly ClientSettings _clientSettings;
    private readonly IClientAccessTokenCache _clientAccessTokenCache;
    private readonly HttpClient _httpClient;
    public ClientCredentialTokenService(IOptions<ServiceAPISettings> serviceAPISettings, IOptions<ClientSettings> clientSettings, IClientAccessTokenCache clientAccessTokenCache, HttpClient httpClient)
    {
        _serviceAPISettings = serviceAPISettings.Value;
        _clientSettings = clientSettings.Value;
        _clientAccessTokenCache = clientAccessTokenCache;
        _httpClient = httpClient;
    }

    public async Task<string> GetToken()
    {
        var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken", null);
        if (currentToken != null)
        {
            return currentToken.AccessToken;
        }
        var discount = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
        {
            Address = _serviceAPISettings.IdentityBaseUri,
        });
        if (discount.IsError)
        {
            throw discount.Exception;
        }
        var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
        {
            ClientId = _clientSettings.WebClient.ClientId,
            ClientSecret = _clientSettings.WebClient.ClientSecret,
            Address = discount.TokenEndpoint
        };
        var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);

        if (newToken.IsError)
        {
            throw newToken.Exception;
        }

        await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken, newToken.ExpiresIn, null);
        return newToken.AccessToken;
    }
}
