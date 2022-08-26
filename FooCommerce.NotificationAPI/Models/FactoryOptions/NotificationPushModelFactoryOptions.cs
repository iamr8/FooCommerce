using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Models.FactoryOptions;

public record NotificationPushModelFactoryOptions : INotificationCommunicationFactoryOptions
{
    public string WebsiteUrl { get; set; }
}