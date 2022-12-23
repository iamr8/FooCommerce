using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FooCommerce.CatalogService.Models;

[Serializable]
public record UpdateCatalogOrderReq
{
    [Required, JsonRequired, JsonPropertyName("id")]
    public int Id { get; set; }
    [Required, JsonRequired, JsonPropertyName("order")]
    public int Order { get; set; }
}