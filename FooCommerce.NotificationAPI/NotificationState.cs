using MassTransit;

namespace FooCommerce.NotificationAPI;

public class NotificationState : SagaStateMachineInstance
{
    public Guid NotificationId { get; set; }
    public int CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
}