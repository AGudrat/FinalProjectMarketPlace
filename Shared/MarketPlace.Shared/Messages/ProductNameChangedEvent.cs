namespace MarketPlace.Shared.Messages
{
    public class ProductNameChangedEvent
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public string UpdatedName { get; set; }
    }
}
