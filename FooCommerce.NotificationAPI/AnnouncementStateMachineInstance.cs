using MassTransit;

namespace FooCommerce.NotificationAPI;

public class AnnouncementStateMachineInstance : SagaStateMachineInstance
{
    public int CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
}