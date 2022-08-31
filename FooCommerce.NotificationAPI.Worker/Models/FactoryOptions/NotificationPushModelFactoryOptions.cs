using FooCommerce.NotificationAPI.Worker.Interfaces;

namespace FooCommerce.NotificationAPI.Worker.Models.FactoryOptions;

public record NotificationPushModelFactoryOptions : INotificationCommunicationFactoryOptions
{
    public string WebsiteUrl { get; set; }
}