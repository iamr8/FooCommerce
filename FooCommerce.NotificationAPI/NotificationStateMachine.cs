using FooCommerce.NotificationAPI.Contracts;

using MassTransit;

namespace FooCommerce.NotificationAPI;

public class NotificationStateMachine
    : MassTransitStateMachine<NotificationStateMachineInstance>
{
    public NotificationStateMachine()
    {
        InstanceState(x => x.CurrentState, Sent, Delivered, Authorized);

        Event(() => SendAnnouncement, x =>
        {
            x.OnMissingInstance(m =>
                m.ExecuteAsync(context => context.RespondAsync<AnnouncementNotFound>(new { context.Message.AnnouncementId })));

            x.CorrelateById(context => context.Message.AnnouncementId);
        });


        Initially(
            When(SendAnnouncement)
                .TransitionTo(Sent));
    }

    public State Sent { get; private set; }
    public State Delivered { get; private set; }
    public State Authorized { get; private set; }
    public Event<SendAnnouncement> SendAnnouncement { get; private set; }
    public Event<AnnouncementSent> AnnouncementSent { get; private set; }
}