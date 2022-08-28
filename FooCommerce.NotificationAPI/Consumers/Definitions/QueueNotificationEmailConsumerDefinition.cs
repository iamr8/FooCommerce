using MassTransit;

namespace FooCommerce.NotificationAPI.Consumers.Definitions;

public class QueueNotificationEmailConsumerDefinition :
    ConsumerDefinition<QueueNotificationEmailConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueNotificationEmailConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseInMemoryOutbox();
        endpointConfigurator.PublishFaults = true;
        endpointConfigurator.ConcurrentMessageLimit = 10;
    }
}