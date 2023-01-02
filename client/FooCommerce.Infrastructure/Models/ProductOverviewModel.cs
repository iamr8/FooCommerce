using System.Text.Json.Serialization;

namespace FooCommerce.Infrastructure.Models;

[Serializable]
[JsonSerializable(typeof(UrlModel))]
[JsonSerializable(typeof(IEnumerable<UrlModel>))]
[JsonSerializable(typeof(ProductOverviewPropertiesModel))]
[JsonSerializable(typeof(IEnumerable<ProductColorModel>))]
[JsonSerializable(typeof(ProductPriceModel))]
[JsonSerializable(typeof(RatingModel))]
public record ProductOverviewModel
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("name")]
    public string Name { get; init; }
    [JsonPropertyName("url")]
    public UrlModel Url { get; init; }

    [JsonPropertyName("images")]
    public IEnumerable<UrlModel> Images { get; init; }
    [JsonPropertyName("properties")]
    public ProductOverviewPropertiesModel Properties { get; init; }
    [JsonPropertyName("rating")]
    public RatingModel Rating { get; init; }
    [JsonPropertyName("colors")]
    public IEnumerable<ProductColorModel> Colors { get; init; }
    [JsonPropertyName("price")]
    public ProductPriceModel Price { get; init; }
}