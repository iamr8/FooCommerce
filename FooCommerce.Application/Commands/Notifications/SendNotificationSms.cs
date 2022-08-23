using FooCommerce.Application.Models.Notifications.Options;

using MediatR;

namespace FooCommerce.Application.Commands.Notifications;

public record SendNotificationSms(SendNotificationSmsOptions Options) : INotification;