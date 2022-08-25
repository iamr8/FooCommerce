using FooCommerce.Application.Membership.Enums;

namespace FooCommerce.Application.Notifications.Interfaces;

public interface INotificationTemplate
{
    Guid Id { get; }
    CommunicationType Communication { get; }
}