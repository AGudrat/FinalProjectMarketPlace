using MarketPlace.Catalog.DTOs;
using MarketPlace.Catalog.Models;
using MarketPlace.Shared.Dtos;

namespace MarketPlace.Catalog.Services
{
    public interface IProductServices
    {
        Task<Response<List<ProductDto>>> GetAllAsync();
        Task<Response<ProductDto>> GetByIdAsync(string id);
        Task<Response<List<ProductDto>>> GetAllByUserId(string userId);
        Task<Response<Product>> CreateAsync(ProductCreateDto courceCreateDto);
        Task<Response<NoContent>> UpdateAsync(ProductUpdateDto courceUpdateDto);
        Task<Response<NoContent>> Delete(string id);
    }
}
