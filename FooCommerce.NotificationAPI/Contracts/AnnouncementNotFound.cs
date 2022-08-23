namespace FooCommerce.NotificationAPI.Contracts;

public record AnnouncementNotFound
{
    public Guid AnnouncementId { get; init; }
}