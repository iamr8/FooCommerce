using System.Text.Json;

using EasyCaching.Core;

using Microsoft.Extensions.Logging;

namespace FooCommerce.Core.Caching;

public static class CacheHelper
{
    private static TModel Parse<TModel>(string key, object value, ILogger logger = null) where TModel : class
    {
        var jsonOfResult = value.ToString();

        var result = JsonSerializer.Deserialize<TModel>(jsonOfResult, JsonDefaultSettings.Settings);

        logger?.LogInformation("Cache hit for key {key}", key);
        return result;
    }

    private static async Task<TModel> ParseAsync<TModel>(string key, object value, ILogger logger = null) where TModel : class
    {
        var jsonOfResult = value.ToString();

        TModel result = null;
        await Task.Factory.StartNew(() =>
        {
            result = JsonSerializer.Deserialize<TModel>(jsonOfResult, JsonDefaultSettings.Settings);
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
            jsonOfResult = JsonSerializer.Serialize(model, JsonDefaultSettings.Settings);
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

    public static Task<TModel> GetOrCreateAsync<TModel>(this IEasyCachingProvider cache, string key, Func<Task<TModel>> factory, ILogger logger = null, CancellationToken cancellationToken = default) where TModel : class
    {
        return cache.GetOrCreateAsync(key, factory, logger, null, cancellationToken);
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
            {
                await cache.RemoveAsync(key, cancellationToken);
                return await cache.GetOrCreateAsync(key, factory, logger, cancellationToken);
            }

            return await ParseAsync<TModel>(key, _value.Value, logger);
        }
    }
}