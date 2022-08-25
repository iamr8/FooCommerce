namespace FooCommerce.Application.Notifications.Interfaces;

public interface INotificationSendToCommunicationOptions : INotificationRequestInfo
{
    INotificationOptions Options { get; init; }
    INotificationTemplate Template { get; init; }
    string WebsiteUrl { get; init; }
}