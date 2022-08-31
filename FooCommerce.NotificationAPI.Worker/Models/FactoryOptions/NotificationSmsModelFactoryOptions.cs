using FooCommerce.NotificationAPI.Worker.Interfaces;

namespace FooCommerce.NotificationAPI.Worker.Models.FactoryOptions;

public record NotificationSmsModelFactoryOptions : INotificationCommunicationFactoryOptions
{
    public string WebsiteUrl { get; set; }
}