namespace MarketPlace.Web.ViewModels.Catalog
{
    public class CategoryUpdateInput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
