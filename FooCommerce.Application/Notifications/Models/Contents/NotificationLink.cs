using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.Application.Notifications.Models.Contents;

public record NotificationLink(string Name, string Url) : INotificationContent;