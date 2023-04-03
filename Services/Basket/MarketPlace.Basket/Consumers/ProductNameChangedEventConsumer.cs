using MarketPlace.Basket.Dtos;
using MarketPlace.Basket.Services;
using MarketPlace.Shared.Messages;
using MarketPlace.Shared.Services;
using MassTransit;
using System.Text.Json;

namespace MarketPlace.Basket.Consumers
{
    public class ProductNameChangedEventConsumer : IConsumer<ProductNameChangedEvent>
    {
        private readonly RedisService _redisService;
        private readonly ISharedIdentityService _sharedIdentityService;
        public ProductNameChangedEventConsumer(RedisService redisService, ISharedIdentityService sharedIdentityService)
        {
            _redisService = redisService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task Consume(ConsumeContext<ProductNameChangedEvent> context)
        {
            var existBasket = await _redisService.GetDb().StringGetAsync(context.Message.UserId);
            var response = JsonSerializer.Deserialize<BasketDto>(existBasket);
            response.BasketItems.First(x => x.ProductId == context.Message.ProductId).ProductName = context.Message.UpdatedName;
            await _redisService.GetDb().StringSetAsync(response.UserId, JsonSerializer.Serialize(response));
        }
    }
}
