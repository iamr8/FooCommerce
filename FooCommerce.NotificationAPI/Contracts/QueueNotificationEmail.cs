using FooCommerce.Application.Notifications.Models.Options;

namespace FooCommerce.NotificationAPI.Contracts;

public interface QueueNotificationEmail : INotificationId
{
    SendNotificationEmailOptions Options { get; }
}