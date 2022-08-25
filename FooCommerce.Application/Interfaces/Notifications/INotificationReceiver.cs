using FooCommerce.Application.Models.Membership;

namespace FooCommerce.Application.Interfaces.Notifications;

public interface INotificationReceiver
{
    Guid UserId { get; }
    string Name { get; }
    IEnumerable<UserCommunicationModel> UserCommunications { get; }
}