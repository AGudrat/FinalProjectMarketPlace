using MarketPlace.Shared.Dtos;

namespace MarketPlace.Discount.Services;

public interface IDiscountService
{
    Task<Response<List<Models.Discount>>> GetAll();
    Task<Response<Models.Discount>> GetById(int id);
    Task<Response<NoContent>> Save(Models.Discount discount);
    Task<Response<NoContent>> DeleteById(int id);
    Task<Response<NoContent>> Update(Models.Discount discount);
    Task<Response<Models.Discount>> GetByCode(string code, string userId);


}
