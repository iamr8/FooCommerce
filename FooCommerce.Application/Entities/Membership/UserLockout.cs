using FooCommerce.Domain.Interfaces.Database;

namespace FooCommerce.Application.Entities.Membership;

public record UserLockout
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public byte ReasonType { get; init; }
    public string ReasonMore { get; init; }
    public DateTimeOffset LockedUntil { get; init; }
    public Guid UserId { get; init; }
}