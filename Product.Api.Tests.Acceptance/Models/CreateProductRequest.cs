using System.Text.Json.Serialization;

namespace Product.Api.Tests.Acceptance.Models;

public class CreateProductRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("rrp")]
    public decimal RRP { get; set; }

    [JsonPropertyName("currencyIsoCode")]
    public string CurrencyIsoCode { get; set; }

    [JsonPropertyName("category")]
    public ProductCategory Category { get; set; }
    
    
}