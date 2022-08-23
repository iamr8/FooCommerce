using FooCommerce.Application.Interfaces.Notifications;

namespace FooCommerce.Application.Models.Notifications.Options;

public record NotificationPushModelOptions : INotificationCommunicationOptions
{
    public string WebsiteUrl { get; set; }
}