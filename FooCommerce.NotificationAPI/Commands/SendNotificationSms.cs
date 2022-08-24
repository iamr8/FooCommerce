using FooCommerce.Application.Models.Notifications.Options;

using MediatR;

namespace FooCommerce.NotificationAPI.Commands;

public record SendNotificationSms(SendNotificationSmsOptions Options) : INotification;