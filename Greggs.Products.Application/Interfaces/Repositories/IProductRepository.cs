using Greggs.Products.Domain.Entities;

namespace Greggs.Products.Application.Interfaces.Services;
public interface IProductRepository
{
    IEnumerable<Product> GetAll();
}
