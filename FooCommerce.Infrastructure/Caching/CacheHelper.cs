using EasyCaching.Core;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace FooCommerce.Infrastructure.Caching;

public static class CacheHelper
{
    private static JsonSerializerSettings Settings
    {
        get
        {
            var defaultSettings = JsonCustomization.DefaultSettings.Settings;
            defaultSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            defaultSettings.NullValueHandling = NullValueHandling.Ignore;
            defaultSettings.Formatting = Formatting.None;
            return defaultSettings;
        }
    }

    public static void Clear(this IMemoryCache cache, string key)
    {
        if (cache.TryGetValue(key, out var value))
        {
            cache.Remove(key);
        }
    }

    private static TModel Parse<TModel>(string key, object value, ILogger logger = null) where TModel : class
    {
        var jsonOfResult = value.ToString();

        var result = JsonConvert.DeserializeObject<TModel>(jsonOfResult, Settings);

        logger?.LogInformation("Cache hit for key {key}", key);
        return result;
    }

    private static async Task<TModel> ParseAsync<TModel>(string key, object value, ILogger logger = null) where TModel : class
    {
        var jsonOfResult = value.ToString();

        TModel result = null;
        await Task.Factory.StartNew(() =>
        {
            result = JsonConvert.DeserializeObject<TModel>(jsonOfResult, Settings);
        });

        logger?.LogInformation("Cache hit for key {key}", key);
        return result;
    }

    private static async Task<TModel> SetAsync<TModel>(this IEasyCachingProviderBase cache, string key, Func<Task<TModel>> factory, ILogger logger = null, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where TModel : class
    {
        logger?.LogInformation("Cache miss for key {key}", key);

        var model = await factory();
        var jsonOfResult = (string)null;

        await Task.Factory.StartNew(() =>
        {
            jsonOfResult = JsonConvert.SerializeObject(model, Settings);
        }, cancellationToken);

        logger?.LogInformation("Cache preparing for key {key}", key);

        expiration ??= TimeSpan.FromMinutes(30);
        var set = await cache.TrySetAsync(key, jsonOfResult, expiration.Value, cancellationToken);
        if (!set)
        {
            logger.LogError("Cache preparation failure for key {key}", key);
        }
        else
        {
            logger?.LogInformation("Cache prepared for key {key}", key);
        }
        return model;
    }

    private static async Task<TModel> SetAsync<TModel>(this IMemoryCache cache, string key, Func<Task<TModel>> factory, ILogger logger = null, MemoryCacheEntryOptions options = null, CancellationToken cancellationToken = default) where TModel : class
    {
        logger?.LogInformation("Cache miss for key {key}", key);

        var model = await factory();
        var jsonOfResult = (string)null;

        await Task.Factory.StartNew(() =>
        {
            jsonOfResult = JsonConvert.SerializeObject(model, Settings);
        }, cancellationToken);

        logger?.LogInformation("Cache preparing for key {key}", key);

        options ??= new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(30),
        };
        cache.Set(key, jsonOfResult, options);

        logger?.LogInformation("Cache prepared for key {key}", key);
        return model;
    }

    public static Task<TModel> GetOrCreateAsync<TModel>(this IEasyCachingProvider cache, string key, Func<Task<TModel>> factory, ILogger logger = null, CancellationToken cancellationToken = default) where TModel : class
    {
        return GetOrCreateAsync(cache, key, factory, logger, null, cancellationToken);
    }

    public static ValueTask<TModel> GetOrCreateAsync<TModel>(this IMemoryCache cache, string key, Func<Task<TModel>> factory, ILogger logger = null, CancellationToken cancellationToken = default) where TModel : class
    {
        return GetOrCreateAsync(cache, key, factory, logger, null, cancellationToken);
    }

    public static async Task<TModel> GetOrCreateAsync<TModel>(this IEasyCachingProvider cache, string key, Func<Task<TModel>> factory, ILogger logger = null, TimeSpan? options = null, CancellationToken cancellationToken = default) where TModel : class
    {
        var hasCache = await cache.ExistsAsync(key, cancellationToken);
        if (!hasCache)
        {
            var cached = await cache.SetAsync(key, factory, logger, options, cancellationToken);
            return cached;
        }
        else
        {
            var _value = await cache.GetAsync<TModel>(key, cancellationToken);
            if (!_value.HasValue || _value.IsNull)
                throw new NullReferenceException($"Cache exists for {key}, but it seems returned null.");

            return await ParseAsync<TModel>(key, _value.Value, logger);
        }
    }

    public static async ValueTask<TModel> GetOrCreateAsync<TModel>(this IMemoryCache cache, string key, Func<Task<TModel>> factory, ILogger logger = null, MemoryCacheEntryOptions options = null, CancellationToken cancellationToken = default) where TModel : class
    {
        var hasCache = cache.TryGetValue(key, out var value);
        if (hasCache)
            return Parse<TModel>(key, value, logger);

        var cached = await cache.SetAsync(key, factory, logger, options, cancellationToken);
        return cached;
    }
}