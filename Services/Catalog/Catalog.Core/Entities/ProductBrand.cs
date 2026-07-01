using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entities
{
    public class ProductBrand : BaseEntity
    {
        [BsonElement("name")]
        public string Name { get; private set; }

        public void UpdateName(string name)
        {
            Name = name;
        }
    }
}
