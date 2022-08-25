using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.Application.Notifications.Models.Options;

public record NotificationPushInAppModelOptions : INotificationCommunicationOptions
{
    public string WebsiteUrl { get; set; }
}