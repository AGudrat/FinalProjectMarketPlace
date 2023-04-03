using AutoMapper;
using MarketPlace.Catalog.DTOs;
using MarketPlace.Catalog.Models;

namespace MarketPlace.Catalog.Mapping;

public class GeneralMapper : Profile
{
    public GeneralMapper()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Product, ProductUpdateDto>().ReverseMap();
        CreateMap<Product, ProductCreateDto>().ReverseMap();
    }
}
