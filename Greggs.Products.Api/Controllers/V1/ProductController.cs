using System.Threading.Tasks;
using Asp.Versioning;
using Greggs.Products.Application.Dtos;
using Greggs.Products.Application.Features.Product.Queries.GetProducts;
using Greggs.Products.Application.Features.Product.Queries.GetProductsInEuros;
using Greggs.Products.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers.V1;

[ApiVersion("1.0")]
public class ProductController : BaseApiController
{
    private readonly ILogger<ProductController> _logger;
    public ProductController(ILogger<ProductController> logger, IMediator mediator) : base(mediator)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<Response<PaginatedResult<ProductDto>>>> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetProductsQuery { PageNumber = pageNumber, PageSize = pageSize };
        var response = await Mediator.Send(query);

        if (response.Succeeded)
        {
            return Ok(response);
        }
        else
        {
            _logger.LogError("Error retrieving products: {Message}", response.Message);
            return StatusCode(500, response);
        }
    }

    [HttpGet("euros")]
    public async Task<ActionResult<Response<PaginatedResult<ProductDto>>>> GetInEuros([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetProductsInEurosQuery { PageNumber = pageNumber, PageSize = pageSize };
        var response = await Mediator.Send(query);

        if (response.Succeeded)
        {
            return Ok(response);
        }
        else
        {
            _logger.LogError("Error retrieving products in Euros: {Message}", response.Message);
            return StatusCode(500, response);
        }
    }
}