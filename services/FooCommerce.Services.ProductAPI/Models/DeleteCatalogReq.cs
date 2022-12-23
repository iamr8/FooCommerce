using System.Text.Json.Serialization;

namespace FooCommerce.Services.ProductAPI.Models;

[Serializable]
public record DeleteCatalogReq
{
    [JsonRequired, JsonPropertyName("id")]
    public int CatalogId { get; init; }
}