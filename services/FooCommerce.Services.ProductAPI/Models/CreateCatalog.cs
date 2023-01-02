using System.ComponentModel.DataAnnotations;

namespace FooCommerce.CatalogService.Models;

public record CreateCatalog
{
    [Required]
    public string Name { get; init; }
    public string Description { get; init; }
    public string IconPath { get; init; }
    public int? ParentId { get; init; }
    public string Slug { get; init; }
}