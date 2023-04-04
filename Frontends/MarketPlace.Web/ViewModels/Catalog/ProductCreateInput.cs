namespace MarketPlace.Web.ViewModels.Catalog
{
    public class ProductCreateInput
    {

        public string Name { get; set; }

        public decimal Price { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string MainPhotoUrl { get; set; } = string.Empty;
        public List<string> OtherPhotosUrl { get; set; } = new();

        public IFormFile MainPhoto { get; set; }
        public IFormFileCollection OtherPhotos { get; set; }
        public string Description { get; set; }

        public string CategoryId { get; set; }
    }
}
