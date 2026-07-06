using Catalog.Core.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Application.Responses
{
    public record ProductResponse
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Summary { get; init; }
        public string Description { get; init; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; init; }
        public string ImageFile { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
        public ProductType ProductType { get; init; }
        public ProductBrand ProductBrand { get; init; }
    }
}
