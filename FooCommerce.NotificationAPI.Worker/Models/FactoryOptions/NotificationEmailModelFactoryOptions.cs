using FooCommerce.NotificationAPI.Worker.Interfaces;

namespace FooCommerce.NotificationAPI.Worker.Models.FactoryOptions;

public record NotificationEmailModelFactoryOptions : INotificationCommunicationFactoryOptions
{
    public DateTime LocalDateTime { get; init; }
    public string WebsiteUrl { get; set; }
}