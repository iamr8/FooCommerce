using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.Application.Notifications.Models.Options;

public record NotificationSmsModelOptions : INotificationCommunicationOptions
{
    public string WebsiteUrl { get; set; }
}