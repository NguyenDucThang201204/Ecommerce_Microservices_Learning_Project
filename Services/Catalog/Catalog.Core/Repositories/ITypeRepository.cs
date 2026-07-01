using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    public interface ITypeRepository
    {
        Task<IEnumerable<ProductType>> GetTypesAsync();
        Task<ProductType> GetTypeByIdAsync(string id);
    }
}
