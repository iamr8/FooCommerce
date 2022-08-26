using FooCommerce.Application.Localization.Models;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.NotificationAPI.Dtos;

public record NotificationTemplatePushModel(Guid Id) : INotificationTemplate
{
    public CommunicationType Communication => CommunicationType.Push_Notification;
    public LocalizerValueCollection Subject { get; init; }
    public LocalizerValueCollection Message { get; init; }
}