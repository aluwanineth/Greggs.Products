namespace Greggs.Products.Application.Dtos;

public class ProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
}
