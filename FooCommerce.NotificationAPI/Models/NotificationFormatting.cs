using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Models;

public record NotificationFormatting(string Key, string Value) : INotificationContent;