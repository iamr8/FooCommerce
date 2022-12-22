using System.Text.Json;
using EasyCaching.Core;
using Microsoft.Extensions.Logging;

namespace FooCommerce.Caching;

public class CacheProvider : ICacheProvider
{
    private readonly IEasyCachingProvider _cache;

    public CacheProvider(IEasyCachingProvider cache)
    {
        _cache = cache;
    }

    private static readonly TimeSpan ExpirationFallback = TimeSpan.FromMinutes(30);

    private static async Task<TModel> ParseAsync<TModel>(string key, object value, CacheOptions options = null, CancellationToken cancellationToken = default) where TModel : class
    {
        var jsonOfResult = value.ToString();

        options ??= new CacheOptions();
        // options.SerializerOptions ??= JsonDefaultSettings.Settings;

        TModel result = null;
        await Task.Factory.StartNew(() => result = JsonSerializer.Deserialize<TModel>(jsonOfResult, options.SerializerOptions), cancellationToken);

        options.Logger?.LogInformation("Cache hit for key {key}", key);
        return result;
    }

    private async Task<TModel> SetAsync<TModel>(string key, Func<CancellationToken, Task<TModel>> factory, CacheOptions options = null, CancellationToken cancellationToken = default) where TModel : class
    {
        options ??= new CacheOptions();
        // options.SerializerOptions ??= JsonDefaultSettings.Settings;
        options.Expiration ??= ExpirationFallback;

        options.Logger?.LogInformation("Cache miss for key {key}", key);

        var model = await factory(cancellationToken);
        var jsonOfResult = (string)null;

        await Task.Factory.StartNew(() =>
        {
            jsonOfResult = JsonSerializer.Serialize(model, options.SerializerOptions);
        }, cancellationToken);

        options.Logger?.LogInformation("Cache preparing for key {key}", key);

        var set = await _cache.TrySetAsync(key, jsonOfResult, options.Expiration.Value, cancellationToken);
        if (!set)
        {
            options.Logger?.LogError("Cache preparation failure for key {key}", key);
        }
        else
        {
            options.Logger?.LogInformation("Cache prepared for key {key}", key);
        }
        return model;
    }

    public void Flush()
    {
        _cache.Flush();
    }

    public async Task FlushAsync(CancellationToken cancellationToken = default)
    {
        await _cache.FlushAsync(cancellationToken);
    }

    public async Task<TModel> GetOrCreateAsync<TModel>(string key, Func<CancellationToken, Task<TModel>> factory, CancellationToken cancellationToken = default) where TModel : class
    {
        var hasCache = await _cache.ExistsAsync(key, cancellationToken);
        if (!hasCache)
        {
            var cached = await SetAsync(key, factory, null, cancellationToken);
            return cached;
        }
        else
        {
            var _value = await _cache.GetAsync<TModel>(key, cancellationToken);
            if (!_value.HasValue || _value.IsNull)
            {
                await _cache.RemoveAsync(key, cancellationToken);
                return await GetOrCreateAsync(key, factory, null, cancellationToken);
            }

            return await ParseAsync<TModel>(key, _value.Value, cancellationToken: cancellationToken);
        }
    }

    public async Task<TModel> GetOrCreateAsync<TModel>(string key, Func<CancellationToken, Task<TModel>> factory, CacheOptions options = null, CancellationToken cancellationToken = default) where TModel : class
    {
        var hasCache = await _cache.ExistsAsync(key, cancellationToken);
        if (!hasCache)
        {
            return await SetAsync(key, factory, options, cancellationToken);
        }
        else
        {
            var _value = await _cache.GetAsync<TModel>(key, cancellationToken);
            if (_value.HasValue && !_value.IsNull)
                return await ParseAsync<TModel>(key, _value.Value, cancellationToken: cancellationToken);
            
            await _cache.RemoveAsync(key, cancellationToken);
            return await GetOrCreateAsync(key, factory, options, cancellationToken);

        }
    }
}