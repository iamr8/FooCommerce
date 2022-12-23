namespace FooCommerce.NotificationService.Models;

public record NotificationReceiver
{
    // public Guid UserId { get; init; }
    public string Name { get; init; }
    public string Address { get; init; }
}