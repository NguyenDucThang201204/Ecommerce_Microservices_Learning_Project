using Catalog.Core.Entities;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public class DatabaseSeeder
    {
        // nhớ là static vì phương thức này sẽ được gọi mà không cần khởi tạo đối tượng DatabaseSeeder
        public static async Task SeedAsync(IOptions<DatabaseSettings> options, IMongoClient client)
        {
            var settings = options.Value;
            var mongoDatabase = client.GetDatabase(settings.DatabaseName);
            var products = mongoDatabase.GetCollection<Product>(settings.ProductCollectionName);
            var brands = mongoDatabase.GetCollection<ProductBrand>(settings.BrandCollectionName);
            var types = mongoDatabase.GetCollection<ProductType>(settings.TypeCollectionName);

            var infraDir = Path.GetDirectoryName(typeof(DatabaseSeeder).Assembly.Location)!;
            var SeedBasePath = Path.Combine(infraDir, "Data", "SeedData");

            //Seed Brands
            List<ProductBrand> brandlist = new();
            // ưu tiên dùng lệnh terminal drop container rồi build lại image rồi up lại container
            //await mongoDatabase.DropCollectionAsync(settings.BrandCollectionName);
            if (await brands.CountDocumentsAsync(_ => true) == 0)
            {
                var brandData = await File.ReadAllTextAsync(Path.Combine(SeedBasePath, "brands.json"));
                brandlist = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                await brands.InsertManyAsync(brandlist);
            }
            else
            {
                await brands.Find(_ => true).ToListAsync();
            }

            //Seed Types
            List<ProductType> typelist = new();
            //await mongoDatabase.DropCollectionAsync(settings.TypeCollectionName);
            if (await types.CountDocumentsAsync(_ => true) == 0)
            {
                var typeData = await File.ReadAllTextAsync(Path.Combine(SeedBasePath, "types.json"));
                typelist = JsonSerializer.Deserialize<List<ProductType>>(typeData);
                await types.InsertManyAsync(typelist);
            }
            else
            {
                await types.Find(_ => true).ToListAsync();
            }

            //Seed Products
            List<Product> productlist = new();
            //await mongoDatabase.DropCollectionAsync(settings.ProductCollectionName);
            if (await products.CountDocumentsAsync(_ => true) == 0)
            {
                var productData = await File.ReadAllTextAsync(Path.Combine(SeedBasePath, "products.json"));
                var dtos = JsonSerializer.Deserialize<List<ProductSeedDto>>(productData)!;

                productlist = dtos.Select(d => Product.Create(
                   d.Name, d.Summary, d.Description, d.Price, d.ImageFile,
                   d.Brand, d.Type)).ToList();

                foreach (var product in productlist)
                {
                    // Reset ID to let MongoDB generate a new one
                    product.UpdateId(null);
                    // Default CreatedDate to current time if not set
                    if (product.CreatedDate == default)
                    {
                        product.UpdateCreatedDate(DateTimeOffset.UtcNow);
                    }
                }
                await products.InsertManyAsync(productlist);
            }
        }
    }
}
