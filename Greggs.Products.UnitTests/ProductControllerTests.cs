using Greggs.Products.Api.Controllers.V1;
using Greggs.Products.Application.Dtos;
using Greggs.Products.Application.Features.Product.Queries.GetProducts;
using Greggs.Products.Application.Features.Product.Queries.GetProductsInEuros;
using Greggs.Products.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Greggs.Products.UnitTests;

public class ProductControllerTests
{
    private readonly Mock<ILogger<ProductController>> _mockLogger;
    private readonly Mock<IMediator> _mockMediator;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mockLogger = new Mock<ILogger<ProductController>>();
        _mockMediator = new Mock<IMediator>();

        _controller = new ProductController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WhenQueryIsSuccessful()
    {
        var paginatedResult = new PaginatedResult<ProductDto>
        {
            Items = new List<ProductDto>
            {
                new ProductDto { Name = "Sausage Roll", Price = 1.0m, Currency = "GBP" },
                new ProductDto { Name = "Vegan Sausage Roll", Price = 1.5m, Currency = "GBP" }
            },
            TotalItems = 2,
            PageNumber = 1,
            PageSize = 2
        };

        var response = new Response<PaginatedResult<ProductDto>>(paginatedResult, "Products retrieved successfully.");

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.Get(1, 2);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var responseData = Assert.IsType<Response<PaginatedResult<ProductDto>>>(okResult.Value);

        Assert.True(responseData.Succeeded);
        Assert.Equal("Products retrieved successfully.", responseData.Message);
        Assert.Equal(2, responseData.Data.Items.Count);
        Assert.Equal(1.0m, responseData.Data.Items[0].Price);
        Assert.Equal("Sausage Roll", responseData.Data.Items[0].Name);
    }

    [Fact]
    public async Task Get_ShouldReturn500_WhenQueryFails()
    {
        var response = new Response<PaginatedResult<ProductDto>>("An error occurred while retrieving products.")
        {
            Succeeded = false,
            Errors = new List<string> { "Database error." }
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.Get(1, 2);

        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);

        var responseData = Assert.IsType<Response<PaginatedResult<ProductDto>>>(statusCodeResult.Value);
        Assert.False(responseData.Succeeded);
        Assert.Equal("An error occurred while retrieving products.", responseData.Message);
    }

    [Fact]
    public async Task GetInEuros_ShouldReturnOk_WhenQueryIsSuccessful()
    {
        var paginatedResult = new PaginatedResult<ProductDto>
        {
            Items = new List<ProductDto>
        {
            new ProductDto { Name = "Sausage Roll", Price = 1.11m, Currency = "EUR" },
            new ProductDto { Name = "Vegan Sausage Roll", Price = 1.67m, Currency = "EUR" }
        },
            TotalItems = 2,
            PageNumber = 1,
            PageSize = 2
        };

        var response = new Response<PaginatedResult<ProductDto>>(paginatedResult, "Products retrieved successfully in Euros.");

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetProductsInEurosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.GetInEuros(1, 2);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var responseData = Assert.IsType<Response<PaginatedResult<ProductDto>>>(okResult.Value);

        Assert.True(responseData.Succeeded);
        Assert.Equal("Products retrieved successfully in Euros.", responseData.Message);
        Assert.Equal(2, responseData.Data.Items.Count);
        Assert.Equal(1.11m, responseData.Data.Items[0].Price);
        Assert.Equal("Sausage Roll", responseData.Data.Items[0].Name);
    }

    [Fact]
    public async Task GetInEuros_ShouldReturn500_WhenQueryFails()
    {
        var response = new Response<PaginatedResult<ProductDto>>("An error occurred while retrieving products in Euros.")
        {
            Succeeded = false,
            Errors = new List<string> { "Database timeout." }
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetProductsInEurosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.GetInEuros(1, 2);

        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);

        var responseData = Assert.IsType<Response<PaginatedResult<ProductDto>>>(statusCodeResult.Value);
        Assert.False(responseData.Succeeded);
        Assert.Equal("An error occurred while retrieving products in Euros.", responseData.Message);
    }
}
