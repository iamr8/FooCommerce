using FooCommerce.Application.Localization.Models;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.NotificationAPI.Dtos;

public record NotificationTemplatePushInAppModel(Guid Id) : INotificationTemplate
{
    public CommunicationType Communication => CommunicationType.Push_InApp;
    public LocalizerValueCollection Subject { get; init; }
    public LocalizerValueCollection Message { get; init; }
}