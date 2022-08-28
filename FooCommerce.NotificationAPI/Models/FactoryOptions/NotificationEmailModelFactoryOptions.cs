using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Models.FactoryOptions;

public record NotificationEmailModelFactoryOptions : INotificationCommunicationFactoryOptions
{
    public DateTime LocalDateTime { get; init; }
    public string WebsiteUrl { get; set; }
}