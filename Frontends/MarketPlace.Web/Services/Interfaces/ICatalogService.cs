using MarketPlace.Web.ViewModels.Catalog;

namespace MarketPlace.Web.Services.Interfaces;

public interface ICatalogService
{
    Task<List<ProductViewModel>> GetAllProductsAsync();
    Task<List<CategoryViewModel>> GetAllCategoriesAsync();
    Task<List<ProductViewModel>> GetAllProductByUserIdAsync(string userId);
    Task<ProductViewModel> GetByProductIdAsync(string productId);
    Task<bool> CreateProductAsync(ProductCreateInput productCreateInput);
    Task<bool> UpdateProductAsync(ProductUpdateInput productUpdateInput);
    Task<bool> DeleteProductAsync(string productId);
    Task<bool> CreateCategoryAsync(CategoryCreateInput categoryCreateInput);
    Task<CategoryViewModel> GetByIdCategory(string id);
    Task<bool> UpdateCategoryAsync(CategoryUpdateInput categoryUpdateInput);
    Task<bool> DeleteCategoryAsync(string id);
}
