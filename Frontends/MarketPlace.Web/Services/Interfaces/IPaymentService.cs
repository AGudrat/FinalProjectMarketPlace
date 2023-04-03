using MarketPlace.Web.ViewModels.FakePayment;

namespace MarketPlace.Web.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ReceivePayment(PaymentInfoInput paymentInfoInput);
    }
}
