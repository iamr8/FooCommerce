using FooCommerce.Application.Models.Notifications.Options;

using MediatR;

namespace FooCommerce.NotificationAPI.Commands;

public record SendNotificationEmail(SendNotificationEmailOptions Options) : INotification;