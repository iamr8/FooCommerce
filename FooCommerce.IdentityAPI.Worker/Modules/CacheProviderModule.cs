using FooCommerce.Caching;
using FooCommerce.Common.Configurations;

namespace FooCommerce.IdentityAPI.Worker.Modules;

public class CacheProviderModule : Module
{
    public void Load(IServiceCollection services)
    {
        services.AddCacheProvider();
    }
}