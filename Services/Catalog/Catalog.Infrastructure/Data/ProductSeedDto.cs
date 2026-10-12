using Catalog.Core.Entities;

namespace Catalog.Infrastructure.Data
{
    public record ProductSeedDto(
    string Name,
    string Summary,
    string Description,
    string ImageFile,
    decimal Price,
    ProductType Type,
    ProductBrand Brand);
}
