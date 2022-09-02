using FooCommerce.Domain.Enums;

namespace FooCommerce.NotificationAPI.Models;

public record NotificationReceiverProvider
{
    public Guid UserId { get; init; }
    public string Name { get; init; }
    public IDictionary<CommunicationType, string> Communications { get; init; }
}