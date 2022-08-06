using System.Text;

using FooCommerce.Domain.Services;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace FooCommerce.Infrastructure.Caching;

public class MemoryCacheService : IMemoryCacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<IMemoryCacheService> _logger;

    public MemoryCacheService(IMemoryCache cache, ILogger<IMemoryCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async ValueTask<T> GetOrCreateAsync<T>(string key, Func<ValueTask<T>> factory, CancellationToken cancellationToken = default) where T : class
    {
        var value = _cache.Get(key);
        if (value is null)
        {
            _logger.LogInformation("Cache miss for key {key}", key);

            var model = await factory();
            var jsonOfResult = string.Empty;
            await Task.Factory.StartNew(() =>
                jsonOfResult = JsonConvert.SerializeObject(model), cancellationToken);

            var bytesOfJson = Encoding.UTF8.GetBytes(jsonOfResult);

            _logger.LogInformation("Cache preparing for key {key}", key);

            var options = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30),
            };
            _cache.Set(key, bytesOfJson, options);

            _logger.LogInformation("Cache prepared for key {key}", key);
            return model;
        }
        else
        {
            var jsonOfResult = value.ToString();
            T result = null;
            await Task.Factory.StartNew(() =>
                result = JsonConvert.DeserializeObject<T>(jsonOfResult), cancellationToken);

            _logger.LogInformation("Cache hit for key {key}", key);
            return result;
        }
    }
}