using FooCommerce.Domain.Enums;
using FooCommerce.Localization.Models;
using FooCommerce.NotificationAPI.Worker.Interfaces;

namespace FooCommerce.NotificationAPI.Worker.Dtos;

public record NotificationTemplatePushInAppModel : INotificationTemplate
{
    internal NotificationTemplatePushInAppModel(Guid id)
    {
        this.Id = id;
    }
    public Guid Id { get; }
    public CommunicationType Communication => CommunicationType.Push_InApp;
    public LocalizerValueCollection Subject { get; init; }
    public LocalizerValueCollection Message { get; init; }
    public IDictionary<string, string> Values { get; }
}