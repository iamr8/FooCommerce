using FooCommerce.Application.Interfaces.Notifications;

namespace FooCommerce.Application.Models.Notifications.Options;

public record SendNotificationSmsOptions : INotificationSendToCommunicationOptions
{
    public EndUser RequestInfo { get; init; }
    public INotificationOptions Options { get; init; }
    public INotificationModelFactory Factory { get; init; }
    public INotificationTemplate Template { get; init; }
    public string WebsiteUrl { get; init; }
}