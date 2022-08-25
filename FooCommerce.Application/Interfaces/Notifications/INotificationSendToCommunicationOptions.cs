namespace FooCommerce.Application.Interfaces.Notifications;

public interface INotificationSendToCommunicationOptions : INotificationRequestInfo
{
    INotificationOptions Options { get; init; }
    INotificationTemplate Template { get; init; }
    string WebsiteUrl { get; init; }
}