using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.NotificationAPI.Interfaces;

public interface INotificationCommunicationOptions : INotificationRequestInfo
{
    INotificationModelFactory Factory { get; init; }
    INotificationOptions Options { get; init; }
    INotificationTemplate Template { get; init; }
    string WebsiteUrl { get; init; }
}