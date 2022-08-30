using FooCommerce.Core.Notifications.Contracts;
using FooCommerce.NotificationAPI.Events;

using MassTransit;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable MemberCanBePrivate.Global
namespace FooCommerce.NotificationAPI.Machines;

public class NotificationStateMachine
    : MassTransitStateMachine<NotificationState>
{
    public NotificationStateMachine()
    {
        InstanceState(x => x.CurrentState,
            Queued);

        Event(() => Queue,
            x =>
            {
                x.OnMissingInstance(m =>
                    m.ExecuteAsync(context =>
                        context.RespondAsync<NotificationSendFailed>(new { context.Message.NotificationId })));

                x.CorrelateById(context => context.Message.NotificationId);
            });

        Initially(When(Queue)
            //.Then(x => x.Saga.NotificationId = x.Message.NotificationId)
            .TransitionTo(Queued));
    }

    public State Queued { get; private set; }

    public Event<QueueNotification> Queue { get; private set; }
}