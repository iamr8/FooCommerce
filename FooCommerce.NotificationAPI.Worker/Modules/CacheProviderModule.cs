using FooCommerce.Caching;
using FooCommerce.Common.Configurations;

namespace FooCommerce.NotificationAPI.Worker.Modules;

public class CacheProviderModule : Module
{
    public void Load(IServiceCollection services)
    {
        services.AddCacheProvider();
    }
}