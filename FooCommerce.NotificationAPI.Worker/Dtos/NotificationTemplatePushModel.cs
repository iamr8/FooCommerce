using FooCommerce.Application.Communications.Enums;
using FooCommerce.Application.Localization.Models;
using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Worker.Dtos;

public record NotificationTemplatePushModel : INotificationTemplate
{
    internal NotificationTemplatePushModel(Guid id)
    {
        this.Id = id;
    }
    public Guid Id { get; }
    public CommunicationType Communication => CommunicationType.Push_Notification;
    public LocalizerValueCollection Subject { get; init; }
    public LocalizerValueCollection Message { get; init; }
    public IDictionary<string, string> Values { get; }
}