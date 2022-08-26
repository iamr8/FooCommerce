using FooCommerce.Application.Notifications.Models.Options;

namespace FooCommerce.NotificationAPI.Contracts;

public interface QueueNotificationSms : INotificationId
{
    SendNotificationSmsOptions Options { get; }
}