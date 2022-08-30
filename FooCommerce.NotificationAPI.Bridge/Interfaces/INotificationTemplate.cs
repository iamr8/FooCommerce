using FooCommerce.Application.Membership.Enums;

namespace FooCommerce.NotificationAPI.Bridge.Interfaces;

public interface INotificationTemplate
{
    Guid Id { get; }
    CommunicationType Communication { get; }
    IDictionary<string, string> Values { get; }
}