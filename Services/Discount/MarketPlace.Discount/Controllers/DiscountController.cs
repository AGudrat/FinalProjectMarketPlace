using MarketPlace.Discount.Services;
using MarketPlace.Shared.ControllerBases;
using MarketPlace.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : CustomBaseController
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DiscountController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResult(await _discountService.GetAll());
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            return CreateActionResult(await _discountService.GetById(Id));
        }

        [HttpGet("[action]/{code}")]
        public async Task<IActionResult> GetByCode([FromRoute] string code)
        {
            var userId = _sharedIdentityService.GetUserId;
            return CreateActionResult(await _discountService.GetByCode(code, userId));
        }
        [HttpPost]
        public async Task<IActionResult> Create(Models.Discount discount)
        {
            return CreateActionResult(await _discountService.Save(discount));
        }
        [HttpPut]
        public async Task<IActionResult> Put(Models.Discount discount)
        {
            return CreateActionResult(await _discountService.Update(discount));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResult(await _discountService.DeleteById(id));
        }
    }
}
