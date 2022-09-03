using Autofac;

using FooCommerce.Infrastructure.Bootstrapper.Modules;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Infrastructure.Bootstrapper;

public static class BootstrapperConfigurator
{
    public static void ConfigureAutofac(this ContainerBuilder containerBuilder, IWebHostEnvironment environment, string connectionString)
    {
        containerBuilder.RegisterModule(new AutoFluentValidationModule());
        containerBuilder.RegisterModule(new MvcModule(environment));
        containerBuilder.RegisterModule(new ServicesModule());
        containerBuilder.RegisterModule(new CachingModule());
        containerBuilder.RegisterModule(new ProtectionModule());
        containerBuilder.RegisterModule(new AppDatabaseProviderModule(connectionString, optionsBuilder =>
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