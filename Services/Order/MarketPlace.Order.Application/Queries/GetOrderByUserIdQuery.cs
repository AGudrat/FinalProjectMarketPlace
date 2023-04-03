using MarketPlace.Order.Application.Dtos;
using MarketPlace.Shared.Dtos;
using MediatR;

namespace MarketPlace.Order.Application.Queries
{
    public class GetOrderByUserIdQuery : IRequest<Response<List<OrderDto>>>
    {
        public string UserId { get; set; }
    }
}
