using FooCommerce.Domain.Enums;

namespace FooCommerce.Services.NotificationAPI.Models;

public record NotificationReceiverProvider
{
    public string Name { get; init; }
    public IDictionary<CommType, string> Communications { get; init; }
}