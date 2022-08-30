using FooCommerce.Core.Notifications.Interfaces;

namespace FooCommerce.Core.Notifications.Models;

public record NotificationLink(string Key, string Value) : INotificationContent;