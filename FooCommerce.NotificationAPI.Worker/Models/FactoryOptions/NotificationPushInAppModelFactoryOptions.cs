using FooCommerce.NotificationAPI.Worker.Interfaces;

namespace FooCommerce.NotificationAPI.Worker.Models.FactoryOptions;

public record NotificationPushInAppModelFactoryOptions : INotificationCommunicationFactoryOptions
{
    public string WebsiteUrl { get; set; }
}