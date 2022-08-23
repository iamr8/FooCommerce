using FooCommerce.Application.Enums.Membership;

namespace FooCommerce.Application.Interfaces.Notifications;

public interface INotificationTemplate
{
    Guid Id { get; }
    CommunicationType Communication { get; }
}