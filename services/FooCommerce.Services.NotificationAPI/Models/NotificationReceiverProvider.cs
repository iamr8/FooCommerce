using FooCommerce.Domain.Enums;

namespace FooCommerce.NotificationService.Models;

public record NotificationReceiverProvider
{
    public string Name { get; init; }
    public IDictionary<CommType, string> Communications { get; init; }
}