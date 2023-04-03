using MarketPlace.Shared.Dtos;
using MarketPlace.Web.Services.Interfaces;
using MarketPlace.Web.ViewModels.Discounts;

namespace MarketPlace.Web.Services
{
    public class DiscountService : IDiscountService
    {
        //GetByCode/{code}
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DiscountViewModel> GetDiscount(string discountCode)
        {
            var response = await _httpClient.GetAsync($"discount/getbycode/{discountCode}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var discount = await response.Content.ReadFromJsonAsync<Response<DiscountViewModel>>();
            return discount.Data;
        }
    }
}
