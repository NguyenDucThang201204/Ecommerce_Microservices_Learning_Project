using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries
{
    // Query này sẽ được dùng bởi Handler và được gọi từ Controller
    public record GetAllBrandsQuery : IRequest<IList<BrandResponse>>
    {

    }
}
