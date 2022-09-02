using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Models;

public record NotificationFormatter(string Key, string Value) : INotificationContent;