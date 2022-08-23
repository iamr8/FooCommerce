using FooCommerce.Application.Interfaces.Notifications;

namespace FooCommerce.Application.Models.Notifications.Contents;

public record NotificationFormatting(string Key, string Text) : INotificationContent;