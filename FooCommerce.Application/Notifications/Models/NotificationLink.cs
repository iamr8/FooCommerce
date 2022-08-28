using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.Application.Notifications.Models;

public record NotificationLink(string Name, string Url) : INotificationContent;