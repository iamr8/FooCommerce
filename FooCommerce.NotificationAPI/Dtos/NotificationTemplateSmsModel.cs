using FooCommerce.Application.Localization.Models;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.NotificationAPI.Dtos;

public record NotificationTemplateSmsModel : INotificationTemplate
{
    internal NotificationTemplateSmsModel(Guid id)
    {
        this.Id = id;
    }
    public Guid Id { get; }
    public CommunicationType Communication => CommunicationType.Mobile_Sms;
    public LocalizerValueCollection Text { get; init; }
    public IDictionary<string, string> Values { get; }
}