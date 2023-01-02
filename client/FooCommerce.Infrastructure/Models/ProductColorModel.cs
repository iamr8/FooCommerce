using System.Text.Json.Serialization;

namespace FooCommerce.Infrastructure.Models;

[Serializable]
public record ProductColorModel
{
    [JsonPropertyName("name")]
    public string Name { get; init; }
    [JsonPropertyName("hex_code")]
    public string HexCode { get; init; }
}