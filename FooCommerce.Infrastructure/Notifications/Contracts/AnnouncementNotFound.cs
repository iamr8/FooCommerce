namespace FooCommerce.Infrastructure.Notifications.Contracts;

public record AnnouncementNotFound
{
    public Guid AnnouncementId { get; init; }
}