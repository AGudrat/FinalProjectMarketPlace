using MarketPlace.Catalog.DTOs;
using MarketPlace.Shared.Dtos;

namespace MarketPlace.Catalog.Services
{
    public interface ICategoryService
    {
        Task<Response<List<Models.Category>>> GetAllAsync();
        Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto);
        Task<Response<CategoryDto>> DeleteAsync(string id);
        Task<Response<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto);
        Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}
