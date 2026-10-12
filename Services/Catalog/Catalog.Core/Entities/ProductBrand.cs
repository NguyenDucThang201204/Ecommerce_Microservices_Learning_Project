using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Catalog.Core.Entities
{
    public class ProductBrand : BaseEntity
    {
        [BsonElement("name")]
        [JsonInclude]
        public string Name { get; private set; }

        public void UpdateName(string name)
        {
            Name = name;
        }
    }
}
