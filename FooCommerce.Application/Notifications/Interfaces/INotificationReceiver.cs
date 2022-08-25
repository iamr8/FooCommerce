using FooCommerce.Application.Membership.Models;

namespace FooCommerce.Application.Notifications.Interfaces;

public interface INotificationReceiver
{
    Guid UserId { get; }
    string Name { get; }
    IEnumerable<UserCommunicationModel> UserCommunications { get; }
}