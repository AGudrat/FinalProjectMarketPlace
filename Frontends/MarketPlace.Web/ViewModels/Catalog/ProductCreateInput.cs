namespace MarketPlace.Web.ViewModels.Catalog
{
    public class ProductCreateInput
    {

        public string Name { get; set; }

        public decimal Price { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public IFormFile PhotoFormFile { get; set; }

        public string Description { get; set; }

        public string CategoryId { get; set; }
    }
}
