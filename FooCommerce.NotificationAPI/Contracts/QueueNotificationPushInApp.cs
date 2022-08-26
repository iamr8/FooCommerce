using FooCommerce.Application.Notifications.Models.Options;

namespace FooCommerce.NotificationAPI.Contracts;

public interface QueueNotificationPushInApp : INotificationId
{
    SendNotificationPushInAppOptions Options { get; }
}