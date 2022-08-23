using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Interfaces.Notifications;
using FooCommerce.Application.Models.Localization;

namespace FooCommerce.Application.Dtos.Notifications;

public record NotificationTemplatePushInAppModel(Guid Id) : INotificationTemplate
{
    public CommunicationType Communication => CommunicationType.Push_InApp;
    public LocalizerValueCollection Subject { get; init; }
    public LocalizerValueCollection Message { get; init; }
}