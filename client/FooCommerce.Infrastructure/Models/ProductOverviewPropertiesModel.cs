using System.Text.Json.Serialization;

namespace FooCommerce.Infrastructure.Models;

[Serializable]
public record ProductOverviewPropertiesModel
{
    [JsonPropertyName("is_ad")]
    public bool IsAd { get; init; }
    [JsonPropertyName("free_shipping")]
    public bool FreeShipping { get; init; }
    [JsonPropertyName("is_new")]
    public bool IsNew { get; init; }
    [JsonPropertyName("in_stock")]
    public bool InStock { get; init; }
}