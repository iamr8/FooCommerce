using System.Runtime.CompilerServices;

using FooCommerce.Application.Notifications.Models;

using MassTransit;

namespace FooCommerce.NotificationAPI.Contracts;

public record QueueNotification : INotificationQueued
{
    public Guid NotificationId { get; init; }
    public NotificationOptions Options { get; set; }

    [ModuleInitializer]
    internal static void Init()
    {
        GlobalTopology.Send.UseCorrelationId<QueueNotification>(x => x.NotificationId);
    }
}