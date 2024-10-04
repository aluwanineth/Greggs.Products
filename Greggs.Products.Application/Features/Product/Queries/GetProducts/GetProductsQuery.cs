using Greggs.Products.Application.Dtos;
using Greggs.Products.Application.Wrappers;
using MediatR;

namespace Greggs.Products.Application.Features.Product.Queries.GetProducts;

public class GetProductsQuery : IRequest<Response<PaginatedResult<ProductDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}