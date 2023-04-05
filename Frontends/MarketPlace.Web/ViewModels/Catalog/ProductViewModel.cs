namespace MarketPlace.Web.ViewModels.Catalog
{
    public class ProductViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }

        public string? MainPhotoStockUrl { get; set; }
        public List<string>? OtherPhotosStockUrl { get; set; } = new();

        public string? MainPhotoUrl { get; set; }
        public List<string>? OtherPhotosUrl { get; set; } = new();

        public string Description { get; set; }
        public string? ShortDescription { get => Description.Length > 100 ? Description.Substring(0, 100) + "..." : Description; }
        public DateTime CreatedTime { get; set; }
        public string? CategoryId { get; set; }
        public CategoryViewModel? Category { get; set; }
    }
}
