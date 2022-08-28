using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.NotificationAPI.Events;

public interface NotificationFailed
    : INotificationId
{
    CommunicationType? Gateway { get; }
}