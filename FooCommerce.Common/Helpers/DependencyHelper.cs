using FooCommerce.Common.Configurations;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Common.Helpers;

public static class DependencyHelper
{
    public static void RegisterModule<TModule>(this IServiceCollection serviceCollection, TModule module) where TModule : Module
    {
        module.Load(serviceCollection);
    }
}