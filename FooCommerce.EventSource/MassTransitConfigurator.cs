using System.Reflection;

using MassTransit;

namespace FooCommerce.EventSource;

public static class MassTransitConfigurator
{
    public static void UsingPreferredConfiguration(this IBusRegistrationConfigurator cfg)
    {
        cfg.SetKebabCaseEndpointNameFormatter();
        // cfg.SetInMemorySagaRepositoryProvider();
        cfg.AddMediator();
    }

    public static void ConfigureBus(this IBusRegistrationConfigurator cfg, params Assembly[] assemblies)
    {
        cfg.UsingPreferredConfiguration();
        cfg.AddConsumers(assemblies);
        cfg.UsingPreferredTransport();
    }

    public static void ConfigureBus(this IBusRegistrationConfigurator cfg, Action<IBusRegistrationConfigurator> customConfig, params Assembly[] assemblies)
    {
        cfg.ConfigureBus(assemblies);
        customConfig?.Invoke(cfg);
    }
}