using MassTransit;

namespace FooCommerce.NotificationAPI;

public class NotificationStateMachineInstance : SagaStateMachineInstance
{
    public int CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
}