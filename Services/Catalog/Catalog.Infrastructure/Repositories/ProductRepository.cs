using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private IMongoCollection<Product> _products;
        private IMongoCollection<ProductBrand> _brands;
        private IMongoCollection<ProductType> _types;
        public ProductRepository(IOptions<DatabaseSettings> options, IMongoClient client)
        {
            var settings = options.Value;
            var mongoDatabase = client.GetDatabase(settings.DatabaseName);
            _products = mongoDatabase.GetCollection<Product>(settings.ProductCollectionName);
            _brands = mongoDatabase.GetCollection<ProductBrand>(settings.BrandCollectionName);
            _types = mongoDatabase.GetCollection<ProductType>(settings.TypeCollectionName);
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            await _products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            var deletedProduct = await _products.DeleteOneAsync(p => p.Id == id);
            return deletedProduct.DeletedCount > 0 && deletedProduct.IsAcknowledged;
        }

        public async Task<ProductBrand> GetBrandByIdAsync(string brandId)
        {
            return await _brands.Find(b => b.Id == brandId).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _products.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByBrandAsync(string brandName)
        {
            return await _products.Find(p => p.ProductBrand.Name == brandName).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            // i là ignored case bỏ qua các tường hợp viết hoa và viết thường
            var filter = Builders<Product>.Filter.Regex(p => p.Name, new BsonRegularExpression($".*{name}.*", "i"));
            return await _products.Find(filter).ToListAsync();
        }

        public async Task<Pagination<Product>> GetProductsByPaginationAsync(CatalogSpecParams productParams)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(productParams.Search))
            {
                //  Dùng Regex để tìm kiếm case-insensitive
                // lí do dùng là vì mongoDB không hỗ trợ Contains, StartsWith, EndsWith như SQL nên phải dùng Regex để tìm kiếm
                var searchRegex = new BsonRegularExpression(productParams.Search, "i");
                filter &= builder.Regex(p => p.Name, searchRegex);
            }

            if (!string.IsNullOrEmpty(productParams.BrandId))
            {
                filter &= builder.Eq(p => p.ProductBrand.Id, productParams.BrandId);
            }

            if (!string.IsNullOrEmpty(productParams.TypeId))
            {
                filter &= builder.Eq(p => p.ProductType.Id, productParams.TypeId);
            }

            // Đếm dựa theo filter để lấy tổng số sản phẩm phù hợp với các điều kiện lọc
            var totalItems = await _products.CountDocumentsAsync(filter);
            var data = await ApplyDataFilter(filter, productParams);

            var pagination = new Pagination<Product>();
            var paginationResult = ApplyPagination(pagination, productParams, (int)totalItems, data);

            return paginationResult;
        }

        private async Task<IReadOnlyCollection<Product>> ApplyDataFilter(FilterDefinition<Product> filter, CatalogSpecParams productParams)
        {
            var sortDefinition = Builders<Product>.Sort.Ascending(p => p.Name);
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                sortDefinition = productParams.Sort switch
                {
                    "priceAsc" => Builders<Product>.Sort.Ascending(p => p.Price),
                    "priceDesc" => Builders<Product>.Sort.Descending(p => p.Price),
                    _ => Builders<Product>.Sort.Ascending(p => p.Name)
                };
            }

            return await _products.Find(filter)
                .Sort(sortDefinition)
                .Skip(productParams.PageSize * (productParams.PageIndex - 1))
                .Limit(productParams.PageSize)
                .ToListAsync();

        }

        private Pagination<Product> ApplyPagination(Pagination<Product> pagination, CatalogSpecParams productParams, int totalItems, IReadOnlyCollection<Product> data)
        {
            pagination.SetCount(totalItems);
            pagination.SetData(data);
            pagination.SetPageSize(productParams.PageSize);
            pagination.SetPageIndex(productParams.PageIndex);
            return pagination;
        }
        public async Task<ProductType> GetTypeByIdAsync(string typeId)
        {
            return await _types.Find(t => t.Id == typeId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var updatedProduct = await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return updatedProduct.ModifiedCount > 0 && updatedProduct.IsAcknowledged;
        }
    }
}
