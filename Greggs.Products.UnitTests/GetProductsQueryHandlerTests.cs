using Greggs.Products.Application.Features.Product.Queries.GetProducts;
using Greggs.Products.Application.Interfaces.Services;
using Greggs.Products.Domain.Entities;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Greggs.Products.UnitTests;

public class GetProductsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnSuccessfulResponseWithPaginatedProductDtos()
    {
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(repo => repo.GetAll()).Returns(new List<Product>
        {
             new Product { Name = "Sausage Roll", PriceInPounds = 1.0m },
            new Product { Name = "Vegan Sausage Roll", PriceInPounds = 1.5m },
            new Product { Name = "Chicken Bake", PriceInPounds = 2.0m },
        });

        var handler = new GetProductsQueryHandler(mockRepo.Object);
        var query = new GetProductsQuery { PageNumber = 1, PageSize = 2 };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.True(result.Succeeded);
        Assert.Equal("Products retrieved successfully.", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Items.Count);
        Assert.Equal(3, result.Data.TotalItems);
        Assert.Equal(1, result.Data.PageNumber);
        Assert.Equal(2, result.Data.PageSize);
        Assert.Equal(2, result.Data.TotalPages);
        Assert.Equal("Sausage Roll", result.Data.Items[0].Name);
        Assert.Equal(1.0m, result.Data.Items[0].Price);
        Assert.Equal("GBP", result.Data.Items[0].Currency);
    }
}
