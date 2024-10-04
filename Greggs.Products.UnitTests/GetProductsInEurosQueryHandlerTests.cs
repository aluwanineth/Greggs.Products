using Greggs.Products.Application.Features.Product.Queries.GetProductsInEuros;
using Greggs.Products.Application.Interfaces.Services;
using Greggs.Products.Application.Settings;
using Greggs.Products.Domain.Entities;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Greggs.Products.UnitTests;

public class GetProductsInEurosQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnSuccessfulResponseWithPaginatedProductDtosInEuros()
    {
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(repo => repo.GetAll()).Returns(new List<Product>
        {
            new Product { Name = "Sausage Roll", PriceInPounds = 1.0m },
            new Product { Name = "Vegan Sausage Roll", PriceInPounds = 1.5m },
            new Product { Name = "Chicken Bake", PriceInPounds = 2.0m },
            new Product { Name = "Steak Bake", PriceInPounds = 2.5m },
            new Product { Name = "Yum Yum", PriceInPounds = 1.2m },
            new Product { Name = "Belgian Bun", PriceInPounds = 1.8m }
        });

        var currencySettings = Options.Create(new CurrencySettings { ExchangeRate = 1.11m });

        var handler = new GetProductsInEurosQueryHandler(mockRepo.Object, currencySettings);

        var query = new GetProductsInEurosQuery { PageNumber = 1, PageSize = 2 };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.True(result.Succeeded);
        Assert.Equal("Products retrieved successfully in Euros.", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Items.Count);
        Assert.Equal(6, result.Data.TotalItems);
        Assert.Equal(1, result.Data.PageNumber);
        Assert.Equal(2, result.Data.PageSize);
        Assert.Equal(3, result.Data.TotalPages);
        Assert.Equal("Sausage Roll", result.Data.Items[0].Name);
        Assert.Equal(1.11m, result.Data.Items[0].Price);
        Assert.Equal("EUR", result.Data.Items[0].Currency);
    }
}

