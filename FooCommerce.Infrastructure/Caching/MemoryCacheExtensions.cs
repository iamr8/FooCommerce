using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FooCommerce.Infrastructure.Caching;

public static class MemoryCacheExtensions
{
    private static JsonSerializerSettings Settings =>
        new()
        {
            Formatting = Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            // MaxDepth = 5,
            NullValueHandling = NullValueHandling.Ignore,
        };

    public static void Clear(this IMemoryCache cache, string key)
    {
        if (cache.TryGetValue(key, out var value))
        {
            cache.Remove(key);
        }
    }

    private static TModel Get<TModel>(string key, object value, ILogger logger = null) where TModel : class
    {
        var jsonOfResult = value.ToString();
        var result = JsonConvert.DeserializeObject<TModel>(jsonOfResult, Settings);

        logger?.LogInformation("Cache hit for key {key}", key);
        return result;
    }

    private static async Task<TModel> SetAsync<TModel>(this IMemoryCache cache, string key, Func<Task<TModel>> factory, ILogger logger = null, CancellationToken cancellationToken = default) where TModel : class
    {
        logger?.LogInformation("Cache miss for key {key}", key);

        var model = await factory();
        var jsonOfResult = (string)null;

        await Task.Factory.StartNew(() =>
        {
            jsonOfResult = JsonConvert.SerializeObject(model, Settings);
        }, cancellationToken);

        logger?.LogInformation("Cache preparing for key {key}", key);

        cache.Set(key, jsonOfResult, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(30),
        });

        logger?.LogInformation("Cache prepared for key {key}", key);
        return model;
    }

    public static async ValueTask<TModel> GetOrCreateAsync<TModel>(this IMemoryCache cache, string key, Func<Task<TModel>> factory, ILogger logger = null, CancellationToken cancellationToken = default) where TModel : class
    {
        var hasCache = cache.TryGetValue(key, out var value);
        if (!hasCache)
        {
            var cached = await cache.SetAsync(key, factory, logger, cancellationToken);
            return cached;
        }
        else
        {
            return Get<TModel>(key, value, logger);
        }
    }
}