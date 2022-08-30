using FooCommerce.NotificationAPI.Bridge.Interfaces;

namespace FooCommerce.NotificationAPI.Bridge.Models;

public record NotificationFormatting(string Key, string Value) : INotificationContent;