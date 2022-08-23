using FooCommerce.Application.Models.Notifications.Options;

using MediatR;

namespace FooCommerce.Application.Commands.Notifications;

public record SendNotificationEmail(SendNotificationEmailOptions Options) : INotification;