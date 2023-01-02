using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FooCommerce.CatalogService.RestModels;

[Serializable]
public record UpdateCatalogVisibilityReq
{
    [Required, JsonRequired, JsonPropertyName("id")]
    public int Id { get; set; }

    [Required, JsonRequired, JsonPropertyName("visible")]
    public bool Visible { get; set; }
}