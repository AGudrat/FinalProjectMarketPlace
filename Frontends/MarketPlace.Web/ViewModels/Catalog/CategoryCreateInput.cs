namespace MarketPlace.Web.ViewModels.Catalog
{
    public class CategoryCreateInput
    {
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public IFormFile Photo { get; set; }
    }
}
