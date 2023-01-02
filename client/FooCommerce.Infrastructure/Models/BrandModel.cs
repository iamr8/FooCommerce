using System.Text.Json.Serialization;

namespace FooCommerce.Infrastructure.Models;

public record BrandModel
{
    [JsonPropertyName("slug")]
    public string Slug { get; init; }
    [JsonPropertyName("name")]
    public string Name { get; init; }
    [JsonPropertyName("url")]
    public UrlModel Url { get; init; }
}