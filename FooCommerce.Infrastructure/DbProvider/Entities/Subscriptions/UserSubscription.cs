using FooCommerce.Domain;

namespace FooCommerce.Infrastructure.DbProvider.Entities.Subscriptions;
#nullable enable
public record UserSubscription
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsSuspended { get; init; }
    public bool IsTrial { get; init; }
    public bool IsCancelled { get; init; }
    public DateTimeOffset? ValidUntil { get; init; }
    public ushort? CancellationReason { get; init; }
    public string? CancellationMore { get; init; }
    public ushort? DeactivationReason { get; init; }
    public string? DeactivationMore { get; init; }
    public Guid PlanId { get; init; }
    public Guid UserId { get; init; }
}