using System.Text.Json.Serialization;

namespace FooCommerce.Infrastructure.Models;

[Serializable]
[JsonSerializable(typeof(CatalogOverviewModel))]
[JsonSerializable(typeof(IEnumerable<CatalogBreadcrumbModel>))]
public record CatalogProductsModel : _ProductsCollection
{
    [JsonPropertyName("catalog")]
    public CatalogOverviewModel Catalog { get; set; }

    [JsonPropertyName("breadcrumbs")]
    public IEnumerable<CatalogBreadcrumbModel> Breadcrumbs { get; set; }
}