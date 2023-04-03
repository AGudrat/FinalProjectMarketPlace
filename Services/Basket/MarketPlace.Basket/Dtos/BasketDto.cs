namespace MarketPlace.Basket.Dtos;

public class BasketDto
{
    public string UserId { get; set; } = null!;
    public string? DiscountCode { get; set; }
    public List<BasketItemDto>? BasketItems { get; set; }
    public int? DiscountRate { get; set; }
    public decimal? TotalPrice
    {
        get => BasketItems.Sum(x => x.Price * x.Quantity);
    }
}
