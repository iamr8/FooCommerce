using FooCommerce.Application.Interfaces.Notifications;

namespace FooCommerce.Application.Models.Notifications.Options;

public record NotificationSmsModelOptions : INotificationCommunicationOptions
{
    public string WebsiteUrl { get; set; }
}