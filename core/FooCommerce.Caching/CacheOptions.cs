using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace FooCommerce.Caching;

public record CacheOptions
{
    public ILogger Logger { get; set; }
    public JsonSerializerOptions SerializerOptions { get; set; }
    public TimeSpan? Expiration { get; set; }
}