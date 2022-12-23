using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FooCommerce.CatalogService.Models;

[Serializable]
public record CreateCatalogReq
{
    [Required, JsonRequired, JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("icon")]
    public string Icon { get; set; }
    [JsonPropertyName("desc")]
    public string Description { get; set; }
    [JsonPropertyName("parentId")]
    public int? ParentId { get; set; }
}