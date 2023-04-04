namespace MarketPlace.Web.ViewModels.Basket
{
    public class BasketItemViewModel
    {
        public int Quantity { get; set; } = 1;
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Picture { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountAppliedPrice;
        public decimal GetCurrentPrice { get => DiscountAppliedPrice is not null ? DiscountAppliedPrice.Value : Price; }
        public void AppliedDiscount(decimal discountPrice)
        {
            DiscountAppliedPrice = discountPrice;
        }
    }
}
