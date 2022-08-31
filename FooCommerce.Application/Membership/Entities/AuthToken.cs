using FooCommerce.Application.Membership.Enums;
using FooCommerce.Domain;

namespace FooCommerce.Application.Membership.Entities;

public record AuthToken
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public AuthTokenAction Action { get; init; }
    public string Token { get; init; }
    public DateTimeOffset? Authorized { get; init; }
    public Guid UserNotificationId { get; init; }
    public Guid UserCommunicationId { get; init; }
}