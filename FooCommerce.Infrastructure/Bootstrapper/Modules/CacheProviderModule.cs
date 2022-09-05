using FooCommerce.Caching;
using FooCommerce.Common.Configurations;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Bootstrapper.Modules;

public class CacheProviderModule : Module
{
    public void Load(IServiceCollection services)
    {
        services.AddCacheProvider();
    }
}