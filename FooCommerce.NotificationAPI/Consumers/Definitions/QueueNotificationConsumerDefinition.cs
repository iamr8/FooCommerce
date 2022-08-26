using MassTransit;
using MassTransit.Configuration;

namespace FooCommerce.NotificationAPI.Consumers.Definitions;

public class QueueNotificationConsumerDefinition :
    ConsumerDefinition<QueueNotificationConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueNotificationConsumer> consumerConfigurator)
    {
        // endpointConfigurator.UseRawJsonSerializer();
        // endpointConfigurator.UseRawJsonDeserializer();

        endpointConfigurator.AddSerializer(new SystemTextJsonMessageSerializerFactory());
        endpointConfigurator.UseInMemoryOutbox();
        endpointConfigurator.PublishFaults = true;
        endpointConfigurator.ConcurrentMessageLimit = 10;
    }
}