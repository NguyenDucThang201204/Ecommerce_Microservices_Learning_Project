using Catalog.Application.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;

namespace Catalog.Application.Mappers
{
    public static class ProductMapper
    {
        public static ProductResponse ToResponse(this Product product)
        {
            if (product == null)
            {
                return null;
            }
            else
            {
                return new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Summary = product.Summary,
                    Description = product.Description,
                    Price = product.Price,
                    ImageFile = product.ImageFile,
                    CreatedDate = product.CreatedDate,
                    ProductType = product.ProductType,
                    ProductBrand = product.ProductBrand
                };
            }
        }

        public static Pagination<ProductResponse> ToResponse(this Pagination<Product> pagination)
            => new Pagination<ProductResponse>(
                pagination.PageIndex,
                pagination.PageSize,
                pagination.Count,
                pagination.Data.Select(p => p.ToResponse()).ToList()
                );

        public static IList<ProductResponse> ToResponseList(this IEnumerable<Product> products)
            => products.Select(p => p.ToResponse()).ToList();

        public static Product ToEntity(this CreateProductCommand command, ProductBrand brand, ProductType type)
            => Product.Create(command.Name, command.Summary, command.Description, command.Price, command.ImageFile, brand, type);
    }
}
