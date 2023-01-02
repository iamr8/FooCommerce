using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.CatalogService.Models;

public record ProductSearchModel : PagedListFilter
{
    [FromQuery(Name = "catalogId")]
    public int CatalogId { get; set; }
}