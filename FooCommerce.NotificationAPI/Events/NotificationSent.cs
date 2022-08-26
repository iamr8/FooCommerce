using FooCommerce.Application.Membership.Enums;
using FooCommerce.NotificationAPI.Contracts;

namespace FooCommerce.NotificationAPI.Events;

public interface NotificationSent : INotificationId
{
    CommunicationType Gateway { get; }
}