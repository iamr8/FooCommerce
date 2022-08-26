using FooCommerce.Application.Models;
using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.Application.Notifications.Models.Options;

public record SendNotificationPushInAppOptions : INotificationSendToCommunicationOptions
{
    public IEndUser RequestInfo { get; set; }
    public INotificationOptions Options { get; init; }
    public INotificationModelFactory Factory { get; init; }
    public INotificationTemplate Template { get; init; }
    public string WebsiteUrl { get; init; }
}