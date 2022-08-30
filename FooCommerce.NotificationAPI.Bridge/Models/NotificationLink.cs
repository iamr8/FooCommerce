using FooCommerce.NotificationAPI.Bridge.Interfaces;

namespace FooCommerce.NotificationAPI.Bridge.Models;

public record NotificationLink(string Key, string Value) : INotificationContent;