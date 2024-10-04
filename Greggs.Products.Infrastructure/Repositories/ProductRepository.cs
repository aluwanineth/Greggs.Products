using Greggs.Products.Application.Interfaces.Services;
using Greggs.Products.Domain.Entities;

namespace Greggs.Products.Infrastructure.Services;
public class ProductRepository : IProductRepository
{
    private static readonly IEnumerable<Product> ProductDatabase = new List<Product>()
        {
            new() { Name = "Sausage Roll", PriceInPounds = 1m },
            new() { Name = "Vegan Sausage Roll", PriceInPounds = 1.1m },
            new() { Name = "Steak Bake", PriceInPounds = 1.2m },
            new() { Name = "Yum Yum", PriceInPounds = 0.7m },
            new() { Name = "Pink Jammie", PriceInPounds = 0.5m },
            new() { Name = "Mexican Baguette", PriceInPounds = 2.1m },
            new() { Name = "Bacon Sandwich", PriceInPounds = 1.95m },
            new() { Name = "Coca Cola", PriceInPounds = 1.2m }
        };

    public IEnumerable<Product> GetAll()
    {
        return ProductDatabase;
    }
}
