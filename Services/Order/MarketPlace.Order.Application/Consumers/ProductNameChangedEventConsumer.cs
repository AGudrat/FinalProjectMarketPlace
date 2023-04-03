using MarketPlace.Order.Infrastructure;
using MarketPlace.Shared.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Order.Application.Consumers
{
    public class ProductNameChangedEventConsumer : IConsumer<ProductNameChangedEvent>
    {
        private readonly OrderDbContext _context;

        public ProductNameChangedEventConsumer(OrderDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<ProductNameChangedEvent> context)
        {
            var orderItems = await _context.OrderItems.Where(x => x.ProductId == context.Message.ProductId).ToListAsync();
            orderItems.ForEach(x =>
            {
                x.UpdateOrderItem(context.Message.UpdatedName, x.PictureUrl, x.Price);
            });
            await _context.SaveChangesAsync();
        }
    }
}
