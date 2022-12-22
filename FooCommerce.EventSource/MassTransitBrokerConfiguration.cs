using MassTransit;

namespace FooCommerce.EventSource;

public class MassTransitBrokerConfiguration
{
    public Action<IBusRegistrationContext, IInMemoryBusFactoryConfigurator> TransportConfig { get; set; }
    public Action<IBusRegistrationConfigurator> BusConfig { get; set; }
}