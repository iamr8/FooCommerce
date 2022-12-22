using MassTransit;
using MassTransit.Serialization;

namespace FooCommerce.EventSource;

public static class MassTransitConfigurator
{
    public static void ConfigureBus(this IBusRegistrationConfigurator busConfig, Action<MassTransitBrokerConfiguration> brokerConfig = null)
    {
        busConfig.AddDelayedMessageScheduler();

        var _brokerConfig = new MassTransitBrokerConfiguration();
        brokerConfig(_brokerConfig);

        busConfig.SetKebabCaseEndpointNameFormatter();

        busConfig.UsingInMemory((context, inMemoryConfig) =>
        {
            inMemoryConfig.UseDelayedMessageScheduler();

            inMemoryConfig.ClearSerialization();
            inMemoryConfig.UseRawJsonSerializer(RawSerializerOptions.AddTransportHeaders | RawSerializerOptions.CopyHeaders);

            inMemoryConfig.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10)));
            inMemoryConfig.UseMessageRetry(r => r.Immediate(3));
            inMemoryConfig.UseInMemoryOutbox();

            inMemoryConfig.AutoStart = true;

            _brokerConfig.TransportConfig?.Invoke(context, inMemoryConfig);

            inMemoryConfig.ConfigureEndpoints(context);
        });

        _brokerConfig.BusConfig?.Invoke(busConfig);
    }
}