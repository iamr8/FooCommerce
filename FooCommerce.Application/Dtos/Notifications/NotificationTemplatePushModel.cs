using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Interfaces.Notifications;
using FooCommerce.Application.Models.Localization;

namespace FooCommerce.Application.Dtos.Notifications;

public record NotificationTemplatePushModel(Guid Id) : INotificationTemplate
{
    public CommunicationType Communication => CommunicationType.Push_Notification;
    public LocalizerValueCollection Subject { get; init; }
    public LocalizerValueCollection Message { get; init; }
}