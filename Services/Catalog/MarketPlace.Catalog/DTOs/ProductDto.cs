﻿namespace MarketPlace.Catalog.DTOs
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string MainPhotoUrl { get; set; }
        public List<string> OtherPhotosUrl { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CategoryId { get; set; }
        public CategoryDto Category { get; set; }
    }
}
