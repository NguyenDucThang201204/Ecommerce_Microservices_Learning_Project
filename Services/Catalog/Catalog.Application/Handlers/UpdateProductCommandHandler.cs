using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(request.Id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with Id {request.Id} not found");
            }

            //Step 1: Fetch Brand and Type
            var productBrand = await _productRepository.GetBrandByIdAsync(request.BrandId);
            var productType = await _productRepository.GetTypeByIdAsync(request.TypeId);
            if (productBrand == null || productType == null)
            {
                throw new ApplicationException("Invalid Brand or Type specified");
            }

            //Step 2: Mapper Role
            var updatedProduct = request.toUpdateEntity(existingProduct, productBrand, productType);

            //Step 3: Save the record
            return await _productRepository.UpdateProductAsync(updatedProduct);
        }
    }
}
