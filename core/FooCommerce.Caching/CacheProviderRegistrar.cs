﻿using EasyCaching.InMemory;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Caching;

public static class CacheProviderRegistrar
{
    public static IServiceCollection AddCacheProvider(this IServiceCollection services)
    {
        // services.AddMemoryCache();
        services.AddEasyCaching(options =>
        {
            options.UseInMemory(config =>
            {
                config.DBConfig = new InMemoryCachingOptions
                {
                    // scan time, default value is 60s
                    ExpirationScanFrequency = 60,
                    // total count of cache items, default value is 10000
                    SizeLimit = 100
                };
                // the max random second will be added to cache's expiration, default value is 120
                config.MaxRdSecond = 120;
                // whether enable logging, default is false
                config.EnableLogging = false;
                // mutex key's alive time(ms), default is 5000
                config.LockMs = 5000;
                // when mutex key alive, it will sleep some time, default is 300
                config.SleepMs = 300;
            }, "foomem");
        });
        services.AddSingleton<ICacheProvider, CacheProvider>();

        return services;
    }
}