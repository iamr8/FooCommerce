using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.Application.Notifications.Models.Contents;

public record NotificationFormatting(string Key, string Text) : INotificationContent;