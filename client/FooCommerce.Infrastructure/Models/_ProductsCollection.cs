using System.Text.Json.Serialization;

namespace FooCommerce.Infrastructure.Models;

[Serializable]
[JsonSerializable(typeof(PaginationModel))]
[JsonSerializable(typeof(IEnumerable<ProductOverviewModel>))]
public abstract record _ProductsCollection
{
    [JsonPropertyName("products")]
    public IEnumerable<ProductOverviewModel> Products { get; set; }
    [JsonPropertyName("filters")]
    public IProductFilter Filters { get; set; }
    [JsonPropertyName("pager")]
    public PaginationModel Pager { get; set; }
}