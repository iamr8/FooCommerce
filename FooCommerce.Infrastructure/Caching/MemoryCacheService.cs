using System.Text;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace FooCommerce.Infrastructure.Caching;

public static class MemoryCacheService
{
    public static async ValueTask<TModel> GetOrCreateAsync<TModel>(this IMemoryCache _cache, string key, Func<ValueTask<TModel>> factory, ILogger logger = null, CancellationToken cancellationToken = default) where TModel : class
    {
        var value = _cache.Get(key);
        if (value is null)
        {
            logger?.LogInformation("Cache miss for key {key}", key);

            var model = await factory();
            var jsonOfResult = string.Empty;
            await Task.Factory.StartNew(() =>
                jsonOfResult = JsonConvert.SerializeObject(model), cancellationToken);

            var bytesOfJson = Encoding.UTF8.GetBytes(jsonOfResult);

            logger?.LogInformation("Cache preparing for key {key}", key);

            var options = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30),
            };
            _cache.Set(key, bytesOfJson, options);

            logger?.LogInformation("Cache prepared for key {key}", key);
            return model;
        }
        else
        {
            var jsonOfResult = value.ToString();
            TModel result = null;
            await Task.Factory.StartNew(() =>
                result = JsonConvert.DeserializeObject<TModel>(jsonOfResult), cancellationToken);

            logger?.LogInformation("Cache hit for key {key}", key);
            return result;
        }
    }
}