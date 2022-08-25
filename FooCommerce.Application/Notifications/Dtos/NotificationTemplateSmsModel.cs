using FooCommerce.Application.Localization.Models;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.Application.Notifications.Dtos;

public record NotificationTemplateSmsModel(Guid Id) : INotificationTemplate
{
    public CommunicationType Communication => CommunicationType.Mobile_Sms;
    public LocalizerValueCollection Text { get; init; }

    public void GetModel()
    {
    }
}