using FooCommerce.Common.Helpers;
using FooCommerce.Infrastructure.Bootstrapper.Modules;

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
        services.RegisterModule(new ProtectionModule());
    }
}