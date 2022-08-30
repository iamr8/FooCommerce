using FooCommerce.Core.Notifications.Interfaces;

namespace FooCommerce.Core.Notifications.Models;

public record NotificationFormatting(string Key, string Value) : INotificationContent;