namespace Product.Api.Tests.Acceptance.Models;

public class Product
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name { get; init; }

    public string Description { get; init; }

    public decimal Price { get; init; }

    public decimal RRP { get; init; }

    public string CurrencyIsoCode { get; init; }

    public ProductCategory Category { get; init; }
}