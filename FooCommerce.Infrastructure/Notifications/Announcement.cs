using MassTransit;

namespace FooCommerce.Infrastructure.Notifications;

public class Announcement : SagaStateMachineInstance
{
    public int CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
}