using FooCommerce.Domain;

namespace FooCommerce.DbProvider.Entities.Notifications;

public record UserNotification
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public string RenderedContent { get; init; }
    public DateTime? Sent { get; init; }
    public DateTime? Delivered { get; init; }
    public DateTime? Seen { get; init; }
    public Guid NotificationId { get; init; }
    public Guid UserId { get; init; }
}