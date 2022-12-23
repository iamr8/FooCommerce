using System.Text.Json.Serialization;

namespace FooCommerce.CatalogService.Models;

[Serializable]
public record DeleteCatalogReq
{
    [JsonRequired, JsonPropertyName("id")]
    public int CatalogId { get; init; }
}