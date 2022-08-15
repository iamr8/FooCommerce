using System.Net;

using FooCommerce.Domain.Entities;

namespace FooCommerce.Application.Entities.Membership;

public record AuthToken
    : IEntity, IEntityRequestTrackable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public byte Action { get; init; }
    public IPAddress IPAddress { get; init; }
    public string UserAgent { get; init; }
    public DateTimeOffset? Sent { get; init; }
    public DateTimeOffset? Delivered { get; init; }
    public DateTimeOffset? Authorized { get; init; }
    public Guid UserCommunicationId { get; set; }
}