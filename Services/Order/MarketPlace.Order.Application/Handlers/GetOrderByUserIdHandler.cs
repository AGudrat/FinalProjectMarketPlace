using MarketPlace.Order.Application.Dtos;
using MarketPlace.Order.Application.Mapping;
using MarketPlace.Order.Application.Queries;
using MarketPlace.Order.Infrastructure;
using MarketPlace.Shared.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Order.Application.Handlers
{
    public class GetOrderByUserIdHandler : IRequestHandler<GetOrderByUserIdQuery, Response<List<OrderDto>>>
    {
        private readonly OrderDbContext _context;
        public GetOrderByUserIdHandler(OrderDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<OrderDto>>> Handle(GetOrderByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(x => x.OrderItems).Where(x => x.BuyerId == request.UserId).ToListAsync();

            if (!orders.Any()) { return Response<List<OrderDto>>.Success(new List<OrderDto>(), 200); }

            var ordersDto = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);

            return Response<List<OrderDto>>.Success(ordersDto, 200);
        }
    }
}
