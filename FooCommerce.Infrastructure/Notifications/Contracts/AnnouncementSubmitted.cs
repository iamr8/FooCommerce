namespace FooCommerce.Infrastructure.Notifications.Contracts;

public interface AnnouncementSent
{
    Guid AnnouncementId { get; }
}