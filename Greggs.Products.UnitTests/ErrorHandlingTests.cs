using Greggs.Products.Application.Features.Product.Queries.GetProducts;
using Greggs.Products.Application.Interfaces.Services;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Greggs.Products.UnitTests;

public class ErrorHandlingTests
{
    [Fact]
    public async Task Handle_ShouldReturnErrorResponseWhenExceptionOccurs()
    {
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(repo => repo.GetAll()).Throws(new Exception("Test exception"));

        var handler = new GetProductsQueryHandler(mockRepo.Object);
        var query = new GetProductsQuery { PageNumber = 1, PageSize = 10 };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Equal("An error occurred while retrieving products.", result.Message);
        Assert.Single(result.Errors);
        Assert.Equal("Test exception", result.Errors[0]);
        Assert.Null(result.Data);
    }
}
