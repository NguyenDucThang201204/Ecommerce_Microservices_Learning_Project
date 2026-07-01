using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly IMongoCollection<ProductBrand> _brands;

        public BrandRepository(IOptions<DatabaseSettings> options, IMongoClient client)
        {
            var settings = options.Value;
            var database = client.GetDatabase(settings.DatabaseName);
            _brands = database.GetCollection<ProductBrand>(settings.BrandCollectionName);
        }
        public async Task<ProductBrand> GetBrandByIdAsync(string id)
        {
            return await _brands.Find(brand => brand.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductBrand>> GetBrandsAsync()
        {
            return await _brands.Find(brand => true).ToListAsync();
        }
    }
}
