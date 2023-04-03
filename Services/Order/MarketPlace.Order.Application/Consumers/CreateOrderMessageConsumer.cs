using MarketPlace.Order.Infrastructure;
using MarketPlace.Shared.Messages;
using MassTransit;

namespace MarketPlace.Order.Application.Consumers
{
    public class CreateOrderMessageConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderMessageConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            var newAddress = new Domain.OrderAggregate.Address(context.Message.Province, context.Message.District, context.Message.Street, context.Message.ZipCode, context.Message.Line);
            var newOrder = new Domain.OrderAggregate.Order(context.Message.BuyerId, newAddress);

            context.Message.OrderItems.ForEach(x =>
            {
                newOrder.AddOrderItem(x.ProductName, x.ProductId, x.Price, x.PictureUrl);
            });

            await _orderDbContext.Orders.AddAsync(newOrder);
            await _orderDbContext.SaveChangesAsync();
        }
    }
}
