using System.Text.Json.Serialization;

namespace FooCommerce.Infrastructure.Models;

public record ProductPriceModel
{
    public ProductPriceModel()
    {
    }

    public ProductPriceModel(decimal value, string currency)
    {
        Value = value;
        Currency = currency;
    }

    [JsonPropertyName("value")]
    public decimal Value { get; init; }
    [JsonPropertyName("currency")]
    public string Currency { get; init; }
}