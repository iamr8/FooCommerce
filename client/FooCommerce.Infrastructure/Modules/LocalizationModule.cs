using FooCommerce.Common.Configurations;
using FooCommerce.Localization.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Modules;

public class LocalizationModule : Module
{
    public void Load(IServiceCollection services)
    {
        services.AddLocalizer();
    }
}