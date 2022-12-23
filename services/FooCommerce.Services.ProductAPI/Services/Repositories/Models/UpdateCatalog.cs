using System.ComponentModel.DataAnnotations;

namespace FooCommerce.Services.ProductAPI.Services.Repositories.Models;

public record UpdateCatalog
{
    public int Id { get; init; }
    [Required]
    public string Name { get; init; }
    public string Description { get; init; }
    public string IconPath { get; init; }
    public int? ParentId { get; init; }
    public string Slug { get; init; }
}