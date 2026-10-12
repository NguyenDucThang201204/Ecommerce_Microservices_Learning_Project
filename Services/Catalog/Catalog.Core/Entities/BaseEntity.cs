using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Catalog.Core.Entities
{
    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonInclude]
        public string Id { get; private set; }

        public void UpdateId(string id)
        {
            Id = id;
        }
    }
}
