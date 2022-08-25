using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.Application.Notifications.Models.Options;

public record NotificationPushModelOptions : INotificationCommunicationOptions
{
    public string WebsiteUrl { get; set; }
}