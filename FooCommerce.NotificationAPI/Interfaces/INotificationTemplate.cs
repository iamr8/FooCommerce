using FooCommerce.Application.Communications.Enums;

namespace FooCommerce.NotificationAPI.Interfaces;

public interface INotificationTemplate
{
    Guid Id { get; }
    CommunicationType Communication { get; }
    IDictionary<string, string> Values { get; }
}