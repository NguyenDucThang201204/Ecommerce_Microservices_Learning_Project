using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; }
        public string Summary { get; private set; }
        public string Description { get; private set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; private set; }
        public string ImageFile { get; private set; }
        public DateTimeOffset CreatedDate { get; private set; }
        public ProductType ProductType { get; private set; }
        public ProductBrand ProductBrand { get; private set; }

        // constructor cho MongoDB deserialize (nếu cần) hoặc để trống nếu dùng convention pack
        private Product() { }

        private Product(string name, string summary, string description, decimal price,
                         string imageFile, ProductBrand brand, ProductType type, DateTimeOffset createdDate)
        {
            Name = name;
            Summary = summary;
            Description = description;
            Price = price;
            ImageFile = imageFile;
            ProductBrand = brand;
            ProductType = type;
            CreatedDate = createdDate;
        }

        public static Product Create(string name, string summary, string description, decimal price,
                                      string imageFile, ProductBrand brand, ProductType type)
        {
            // validate ngay tại đây => bảo vệ invariant của domain
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required");
            if (price < 0)
                throw new ArgumentException("Price must be >= 0");

            return new Product(name, summary, description, price, imageFile, brand, type, DateTimeOffset.UtcNow);
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));
            Name = name;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void UpdatePrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException("Price must be >= 0", nameof(price));
            Price = price;
        }

        public void UpdateImageFile(string imageFile)
        {
            ImageFile = imageFile;
        }

        public void UpdateSummary(string summary)
        {
            Summary = summary;
        }

        public void UpdateCreatedDate(DateTimeOffset createdDate)
        {
            CreatedDate = createdDate;
        }

        public void UpdateProductType(ProductType productType)
        {
            ProductType = productType;
        }

        public void UpdateProductBrand(ProductBrand productBrand)
        {
            ProductBrand = productBrand;
        }

    }
}
