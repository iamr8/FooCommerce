using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Models;

public record NotificationLink(string Key, string Value) : INotificationContent;