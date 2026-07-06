using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;
using MongoDB.Driver;

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

        public static Product toUpdateEntity(this UpdateProductCommand command, Product existingProduct, ProductBrand brand, ProductType type)
        {
            existingProduct.UpdateName(command.Name);
            existingProduct.UpdateSummary(command.Summary);
            existingProduct.UpdateImageFile(command.ImageFile);
            existingProduct.UpdateDescription(command.Description);
            existingProduct.UpdatePrice(command.Price);
            existingProduct.UpdateProductBrand(brand);
            existingProduct.UpdateProductType(type);
            return existingProduct;
        }

        public static ProductDTO ToDto(this ProductResponse product)
        {
            if (product == null)
            {
                return null;
            }
            return new ProductDTO
            (
                product.Id,
                product.Name,
                product.Summary,
                product.Description,
                product.ImageFile,
                new BrandDTO(product.ProductBrand.Id, product.ProductBrand.Name),
                new TypeDTO(product.ProductType.Id, product.ProductType.Name),
                product.Price,
                DateTimeOffset.UtcNow
            );
        }
    }
}
