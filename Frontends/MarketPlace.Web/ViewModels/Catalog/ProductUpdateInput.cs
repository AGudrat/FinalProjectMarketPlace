namespace MarketPlace.Web.ViewModels.Catalog
{
    public class ProductUpdateInput
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public string MainPhotoUrl { get; set; } = string.Empty;
        public List<string> OtherPhotosUrl { get; set; } = new();

        public IFormFile? MainPhoto { get; set; }
        public IFormFileCollection? OtherPhotos { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string CategoryId { get; set; }
    }
}
