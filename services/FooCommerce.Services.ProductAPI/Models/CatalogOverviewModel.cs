using System.Text.Json.Serialization;

namespace FooCommerce.CatalogService.Models;

public record CatalogOverviewModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("desc")]
    public string Description { get; set; }
}