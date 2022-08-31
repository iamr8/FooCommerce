using System.Net;

using FooCommerce.Domain;

namespace FooCommerce.NotificationAPI.Worker.DbProvider.Entities;

public record UserNotification
    : IEntity, IEntityRequestTrackable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public string RenderedContent { get; init; }
    public IPAddress IPAddress { get; init; }
    public string UserAgent { get; init; }
    public DateTimeOffset? Sent { get; init; }
    public DateTimeOffset? Delivered { get; init; }
    public DateTimeOffset? Seen { get; init; }
    public Guid NotificationId { get; init; }
    public Guid UserId { get; init; }
}