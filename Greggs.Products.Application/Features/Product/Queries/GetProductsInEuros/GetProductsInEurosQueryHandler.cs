using Greggs.Products.Application.Dtos;
using Greggs.Products.Application.Interfaces.Services;
using Greggs.Products.Application.Settings;
using Greggs.Products.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Greggs.Products.Application.Features.Product.Queries.GetProductsInEuros;

public class GetProductsInEurosQueryHandler : IRequestHandler<GetProductsInEurosQuery, Response<PaginatedResult<ProductDto>>>
{
    private readonly IProductRepository _productRepository;
    private readonly decimal _exchangeRate;
   // private const decimal ExchangeRate = 1.11m;

    public GetProductsInEurosQueryHandler(IProductRepository productRepository, IOptions<CurrencySettings> currencySettings)
    {
        _productRepository = productRepository;
        _exchangeRate = currencySettings.Value.ExchangeRate;
    }

    public Task<Response<PaginatedResult<ProductDto>>> Handle(GetProductsInEurosQuery request, CancellationToken cancellationToken)
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
                    Price = Math.Round(p.PriceInPounds * _exchangeRate, 2),
                    Currency = "EUR"
                })
                .ToList();

            var paginatedResult = new PaginatedResult<ProductDto>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            return Task.FromResult(new Response<PaginatedResult<ProductDto>>(paginatedResult, "Products retrieved successfully in Euros."));
        }
        catch (Exception ex)
        {
            return Task.FromResult(new Response<PaginatedResult<ProductDto>>("An error occurred while retrieving products in Euros.") { Errors = new List<string> { ex.Message } });
        }
    }
}
