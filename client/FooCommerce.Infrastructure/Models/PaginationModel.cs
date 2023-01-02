using System.Text.Json.Serialization;

namespace FooCommerce.Infrastructure.Models;

public record PaginationModel
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }
}