using MassTransit;

namespace FooCommerce.NotificationAPI.Worker.Consumers.Definitions;

public class QueueNotificationConsumerDefinition :
    ConsumerDefinition<QueueNotificationConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueNotificationConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseInMemoryOutbox();
        endpointConfigurator.PublishFaults = true;
        endpointConfigurator.ConcurrentMessageLimit = 10;
    }
}