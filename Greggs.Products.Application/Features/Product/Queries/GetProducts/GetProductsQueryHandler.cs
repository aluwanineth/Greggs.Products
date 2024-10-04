using Greggs.Products.Application.Dtos;
using Greggs.Products.Application.Interfaces.Services;
using Greggs.Products.Application.Wrappers;
using MediatR;

namespace Greggs.Products.Application.Features.Product.Queries.GetProducts;
public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Response<PaginatedResult<ProductDto>>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<Response<PaginatedResult<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var products = _productRepository.GetAll();
            var totalItems = products.Count();
            var items = products
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProductDto
                {
                    Name = p.Name,
                    Price = p.PriceInPounds,
                    Currency = "GBP"
                })
                .ToList();

            var paginatedResult = new PaginatedResult<ProductDto>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            return Task.FromResult(new Response<PaginatedResult<ProductDto>>(paginatedResult, "Products retrieved successfully."));
        }
        catch (Exception ex)
        {
            return Task.FromResult(new Response<PaginatedResult<ProductDto>>("An error occurred while retrieving products.") { Errors = new List<string> { ex.Message } });
        }
    }
}