using FooCommerce.Domain.Enums;
using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Worker.Enums;

namespace FooCommerce.NotificationAPI.Worker.Events;

public interface NotificationSendFaulted
    : INotificationId
{
    NotificationSentFault Fault { get; }
    CommunicationType? Gateway { get; }
}