using FooCommerce.Application.Membership.Enums;
using FooCommerce.NotificationAPI.Contracts;

namespace FooCommerce.NotificationAPI.Events;

public interface NotificationFailed : INotificationId
{
    CommunicationType? Gateway { get; }
}