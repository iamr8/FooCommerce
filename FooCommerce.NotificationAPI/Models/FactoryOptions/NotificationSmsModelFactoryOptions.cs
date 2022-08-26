using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Models.FactoryOptions;

public record NotificationSmsModelFactoryOptions : INotificationCommunicationFactoryOptions
{
    public string WebsiteUrl { get; set; }
}