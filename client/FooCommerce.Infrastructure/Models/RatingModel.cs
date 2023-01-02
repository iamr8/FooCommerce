using System.Text.Json.Serialization;

namespace FooCommerce.Infrastructure.Models;

[Serializable]
public record RatingModel
{
    public RatingModel()
    {
    }

    public RatingModel(double average, int count)
    {
        Average = average;
        Count = count;
    }

    [JsonPropertyName("average")]
    public double Average { get; init; }
    [JsonPropertyName("count")]
    public int Count { get; init; }
}