using Greggs.Products.Application.Interfaces.Services;
using Greggs.Products.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Greggs.Products.Infrastructure;
public static class ServiceExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
    }
}
