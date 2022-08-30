using FooCommerce.Application.Membership.Enums;
using FooCommerce.NotificationAPI.Bridge.Interfaces;

namespace FooCommerce.NotificationAPI.Events;

public interface NotificationSent
    : INotificationId
{
    CommunicationType Gateway { get; }
}