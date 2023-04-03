using MarketPlace.Order.Application.Commands;
using MarketPlace.Order.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> SaveOrder(CreateOrderCommand createOrderCommand)
        {
            var response = await Mediator.Send(createOrderCommand);
            return CreateActionResult(response);
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await Mediator.Send(new GetOrderByUserIdQuery() { UserId = SharedIdentity.GetUserId });
            return CreateActionResult(response);
        }
    }
}
