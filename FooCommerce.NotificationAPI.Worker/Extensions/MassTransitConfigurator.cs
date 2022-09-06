using FooCommerce.EventSource;

using MassTransit;

namespace FooCommerce.NotificationAPI.Worker.Extensions;

public static class MassTransitConfigurator
{
    public static void ConfigureBus(this IBusRegistrationConfigurator cfg)
    {
        cfg.SetKebabCaseEndpointNameFormatter();
        // cfg.SetInMemorySagaRepositoryProvider();
        cfg.AddMediator();

        cfg.AddConsumers(typeof(Worker).Assembly);

        cfg.UsingPreferredTransport();
    }
}