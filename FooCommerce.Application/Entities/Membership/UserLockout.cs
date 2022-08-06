using FooCommerce.Domain.Entities;

namespace FooCommerce.Application.Entities.Membership;

public record UserLockout
    : IEntity
{
    public Guid Id { get; init; }
    public DateTimeOffset Created { get; init; }
    public byte[] RowVersion { get; init; }
    public ushort ReasonType { get; init; }
    public string ReasonMore { get; init; }
    public DateTimeOffset LockedUntil { get; init; }
    public Guid UserId { get; init; }
}