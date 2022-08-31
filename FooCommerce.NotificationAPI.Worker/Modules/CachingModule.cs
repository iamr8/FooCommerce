using Autofac;
using Autofac.Extensions.DependencyInjection;
using EasyCaching.InMemory;

namespace FooCommerce.NotificationAPI.Worker.Modules
{
    public class CachingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();

            services.AddMemoryCache();
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

            builder.Populate(services);
        }
    }
}