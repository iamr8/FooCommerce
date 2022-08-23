using FooCommerce.Infrastructure.Shopping.Contracts;

using MassTransit;

namespace FooCommerce.Infrastructure.Shopping.StateMachines;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public OrderStateMachine()
    {
        Event(() => AcceptOrder, x =>
        {
            x.OnMissingInstance(m =>
                m.ExecuteAsync(context => context.RespondAsync<OrderNotFound>(new { context.Message.OrderId })));
        });
        Event(() => GetOrder, x =>
        {
            x.OnMissingInstance(m =>
                m.ExecuteAsync(context => context.RespondAsync<OrderNotFound>(new { context.Message.OrderId })));
        });

        InstanceState(x => x.CurrentState);

        Initially(
            When(SubmitOrder)
                .Then(x => x.Saga.OrderNumber = x.Message.OrderNumber)
                .TransitionTo(Submitted));

        During(Submitted, Accepted,
            When(AcceptOrder)
                .TransitionTo(Accepted)
                .RespondAsync(x => x.Init<Order>(new
                {
                    x.Message.OrderId,
                    x.Saga.OrderNumber,
                    Status = x.Saga.CurrentState
                })));

        DuringAny(
            When(SubmitOrder)
                .Then(x => x.Saga.OrderNumber = x.Message.OrderNumber),
            When(GetOrder)
                .RespondAsync(x => x.Init<Order>(new
                {
                    x.Message.OrderId,
                    x.Saga.OrderNumber,
                    Status = x.StateMachine.Accessor.Get(x)
                })));
    }

    // ReSharper disable UnassignedGetOnlyAutoProperty
    // ReSharper disable MemberCanBePrivate.Global
    public State Accepted { get; set; }

    public State Submitted { get; set; }

    public Event<SubmitOrder> SubmitOrder { get; set; }

    public Event<GetOrder> GetOrder { get; set; }

    public Event<AcceptOrder> AcceptOrder { get; set; }
}