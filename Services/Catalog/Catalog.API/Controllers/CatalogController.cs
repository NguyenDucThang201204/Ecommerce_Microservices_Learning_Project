using Catalog.Application.DTOs;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CatalogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<Pagination<ProductDTO>>> GetAllProducts([FromQuery] CatalogSpecParams specParams)
        {
            var query = new GetAllProductsQuery(specParams);
            var result = await _mediator.Send(query);
            var dtoList = result.Data.Select(p => p.ToDto()).ToList();
            var pagination = new Pagination<ProductDTO>(result.PageIndex, result.PageSize, result.Count, dtoList);
            return Ok(pagination);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("productName/{productName}")]
        public async Task<ActionResult<IList<ProductDTO>>> GetProductsByProductName(string productName)
        {
            var query = new GetProductsByNameQuery(productName);
            var result = await _mediator.Send(query);
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            var dtoList = result.Select(p => p.ToDto()).ToList();
            return Ok(dtoList);
        }

    }
}
