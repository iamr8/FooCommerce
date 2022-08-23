using FooCommerce.Application.Interfaces.Notifications;

namespace FooCommerce.Application.Models.Notifications.Contents;

public record NotificationLink(string Name, string Url) : INotificationContent;