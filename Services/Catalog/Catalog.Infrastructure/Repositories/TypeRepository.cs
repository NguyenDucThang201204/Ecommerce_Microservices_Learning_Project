using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class TypeRepository : ITypeRepository
    {
        private IMongoCollection<ProductType> _types;

        public TypeRepository(IOptions<DatabaseSettings> option, IMongoClient client)
        {
            var settings = option.Value;
            var database = client.GetDatabase(settings.DatabaseName);
            _types = database.GetCollection<ProductType>(settings.TypeCollectionName);
        }
        public async Task<ProductType> GetTypeByIdAsync(string id)
        {
            return await _types.Find(types => types.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductType>> GetTypesAsync()
        {
            return await _types.Find(_ => true).ToListAsync();
        }
    }
}
