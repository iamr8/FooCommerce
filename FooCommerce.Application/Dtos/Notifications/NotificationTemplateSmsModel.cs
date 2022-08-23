using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Interfaces.Notifications;
using FooCommerce.Application.Models.Localization;

namespace FooCommerce.Application.Dtos.Notifications;

public record NotificationTemplateSmsModel(Guid Id) : INotificationTemplate
{
    public CommunicationType Communication => CommunicationType.Mobile_Sms;
    public LocalizerValueCollection Text { get; init; }

    public void GetModel()
    {
    }
}