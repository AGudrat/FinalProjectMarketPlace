using AutoMapper;
using MarketPlace.Order.Application.Dtos;

namespace MarketPlace.Order.Application.Mapping;

public class CustomMapping : Profile
{
    public CustomMapping()
    {
        CreateMap<Domain.OrderAggregate.Order, OrderDto>().ReverseMap();
        CreateMap<Domain.OrderAggregate.OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<Domain.OrderAggregate.Address, AddressDto>().ReverseMap();
    }
}
