using System.Text.Json.Serialization;

namespace FooCommerce.Infrastructure.Models;

[Serializable]
public record UrlModel
{
    public UrlModel()
    {
    }

    public UrlModel(string url)
    {
        Url = url;
    }

    [JsonPropertyName("url")]
    public string Url { get; init; }
}