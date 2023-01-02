using System.Text.Json.Serialization;

namespace FooCommerce.Infrastructure.Models;

[Serializable]
[JsonSerializable(typeof(BrandModel))]
public record BrandProductsModel : _ProductsCollection
{
    [JsonPropertyName("brand")]
    public BrandModel Brand { get; set; }
}