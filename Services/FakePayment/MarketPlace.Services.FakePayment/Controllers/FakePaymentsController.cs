using MarketPlace.Services.FakePayment.Models;
using MarketPlace.Shared.ControllerBases;
using MarketPlace.Shared.Dtos;
using MarketPlace.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentsController : CustomBaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public FakePaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Get(PaymentDto paymentDto)
        {
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));
            var createOrderMessageCommand = new CreateOrderMessageCommand()
            {
                BuyerId = paymentDto.Order.BuyerId,
                Province = paymentDto.Order.Address.Province,
                District = paymentDto.Order.Address.District,
                Line = paymentDto.Order.Address.Line,
                Street = paymentDto.Order.Address.Street,
                ZipCode = paymentDto.Order.Address.ZipCode,
            };
            paymentDto.Order.OrderItems.ForEach(x =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItem()
                {
                    PictureUrl = x.PictureUrl,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName
                });
            });

            await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

            return CreateActionResult<NoContent>(Shared.Dtos.Response<NoContent>.Success(200));
        }
    }
}
