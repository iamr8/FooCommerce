using FooCommerce.Application.Membership.Enums;
using FooCommerce.Core.Notifications.Interfaces;

namespace FooCommerce.NotificationAPI.Events;

public interface NotificationSent
    : INotificationId
{
    CommunicationType Gateway { get; }
}