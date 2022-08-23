using FooCommerce.Application.Models;

namespace FooCommerce.Application.Interfaces.Notifications;

public interface INotificationSendToCommunicationOptions
{
    EndUser RequestInfo { get; init; }
    INotificationOptions Options { get; init; }
    INotificationTemplate Template { get; init; }
    string WebsiteUrl { get; init; }
}