using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Models.FactoryOptions;

public record NotificationPushInAppModelFactoryOptions : INotificationCommunicationFactoryOptions
{
    public string WebsiteUrl { get; set; }
}