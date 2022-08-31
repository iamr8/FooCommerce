using FooCommerce.Domain;
using FooCommerce.MembershipAPI.Worker.Enums;

namespace FooCommerce.MembershipAPI.Worker.DbProvider.Entities;

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