namespace MarketPlace.Web.ViewModels.Catalog
{
    public class ProductUpdateInput
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public IFormFile PhotoFormFile { get; set; }
        public string Picture { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string CategoryId { get; set; }
    }
}
