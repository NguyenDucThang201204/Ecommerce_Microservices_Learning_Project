using Catalog.Application.Responses;
using Catalog.Core.Entities;

namespace Catalog.Application.Mappers
{
    public static class BrandMapper
    {
        public static BrandResponse ToResponse(this ProductBrand brand)
        {
            return new BrandResponse
            {
                Id = brand.Id,
                Name = brand.Name
            };
        }

        // Lí do biến truyền vào là IEnumerable vì IEnumeable là generic abstraction cao nhất của collection
        // Mapper thì lại không quan trọng đến collection là gì, chỉ cần biết nó có thể enumerate được là đủ
        public static IList<BrandResponse> ToResponseList(this IEnumerable<ProductBrand> brands)
        {
            return brands.Select(b => b.ToResponse()).ToList();
        }
    }
}
