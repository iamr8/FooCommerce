using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.NotificationAPI.Events;

public interface NotificationSendFailed
    : INotificationId
{
    CommunicationType? Gateway { get; }
}