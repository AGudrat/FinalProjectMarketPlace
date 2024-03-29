﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MarketPlace.Catalog.Models;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Price { get; set; }
    public string UserId { get; set; }
    public string MainPhotoUrl { get; set; }
    public List<string> OtherPhotosUrl { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime CreatedTime { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; }
    [BsonIgnore]
    public Category Category { get; set; }

}
