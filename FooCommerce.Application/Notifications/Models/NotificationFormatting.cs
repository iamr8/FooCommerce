using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.Application.Notifications.Models;

public record NotificationFormatting(string Key, string Text) : INotificationContent;