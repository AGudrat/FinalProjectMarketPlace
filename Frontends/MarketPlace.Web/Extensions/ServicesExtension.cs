using MarketPlace.Web.Handler;
using MarketPlace.Web.Models;
using MarketPlace.Web.Services;
using MarketPlace.Web.Services.Interfaces;

namespace MarketPlace.Web.Extensions;

public static class servicesExtension
{
    public static void AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceApiSettings = configuration.GetSection("ServiceAPISettings").Get<ServiceAPISettings>();
        services.AddHttpClient<IIdentitiyService, IdentityService>();
        services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();
        services.AddHttpClient<IBasketService, BasketService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Basket.Path}/");
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        services.AddHttpClient<ICatalogService, CatalogService>(opt =>
         {
             opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Catalog.Path}/");
         }).AddHttpMessageHandler<ClientCredentialTokenHandler>();
        services.AddHttpClient<IUserService, UserService>(opt =>
         {
             opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
         }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        services.AddHttpClient<IPhotoStockService, PhotoStockService>(opt =>
         {
             opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.PhotoStock.Path}/");
         }).AddHttpMessageHandler<ClientCredentialTokenHandler>();
        services.AddHttpClient<IDiscountService, DiscountService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Discount.Path}/");
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        services.AddHttpClient<IPaymentService, PaymentService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.FakePayment.Path}/");
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        services.AddHttpClient<IOrderService, OrderService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Order.Path}/");
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
    }
}
