using MarketPlace.Order.Application.Commands;
using MarketPlace.Order.Application.Dtos;
using MarketPlace.Order.Domain.OrderAggregate;
using MarketPlace.Order.Infrastructure;
using MarketPlace.Shared.Dtos;
using MediatR;

namespace MarketPlace.Order.Application.Handlers;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
{
    private readonly OrderDbContext _context;

    public CreateOrderHandler(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var newAddress = new Address(request.Address.Province, request.Address.District, request.Address.Street, request.Address.ZipCode, request.Address.Line);

        var newOrder = new Domain.OrderAggregate.Order(request.BuyerId, newAddress);

        request.OrderItems.ForEach(x =>
        {
            newOrder.AddOrderItem(x.ProductName, x.ProductId, x.Price, x.PictureUrl);
        });

        await _context.Orders.AddAsync(newOrder);

        await _context.SaveChangesAsync();

        return Response<CreatedOrderDto>.Success(new() { OrderId = newOrder.Id }, 200);
    }
}
