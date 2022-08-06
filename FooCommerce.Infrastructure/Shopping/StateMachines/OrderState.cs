
using MassTransit;

namespace FooCommerce.Infrastructure.Shopping.StateMachines;

public class OrderState : SagaStateMachineInstance
{
    public string CurrentState { get; set; }
    public string OrderNumber { get; set; }
    public Guid CorrelationId { get; set; }
}