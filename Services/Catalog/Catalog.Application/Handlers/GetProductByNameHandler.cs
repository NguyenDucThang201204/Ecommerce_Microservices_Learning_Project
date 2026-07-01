using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetProductByNameHandler : IRequestHandler<GetProductsByNameQuery, IList<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByNameHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IList<ProductResponse>> Handle(GetProductsByNameQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productRepository.GetProductsByNameAsync(request.name);
            return productList.ToResponseList();
        }
    }
}
