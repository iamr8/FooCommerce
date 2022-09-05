using FooCommerce.Common.Helpers;
using FooCommerce.Infrastructure.Bootstrapper.Modules;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Bootstrapper;

public static class BootstrapperConfigurator
{
    public static void ConfigureModules(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

        services.RegisterModule(new AutoFluentValidationModule());
        services.RegisterModule(new MvcModule());
        services.RegisterModule(new ServicesModule());
        services.RegisterModule(new CacheProviderModule());
        services.RegisterModule(new ProtectionModule());
        services.RegisterModule(new AppDatabaseProviderModule(connectionString, optionsBuilder =>
        {
            optionsBuilder.UseSqlServer(connectionString!,
                config =>
                {
                    config.EnableRetryOnFailure(3);
                    config.UseNetTopologySuite();
                    config.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
        }));
    }
}