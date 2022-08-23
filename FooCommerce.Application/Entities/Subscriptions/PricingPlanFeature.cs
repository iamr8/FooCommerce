using FooCommerce.Domain.Interfaces.Database;

namespace FooCommerce.Application.Entities.Subscriptions;

public record PricingPlanFeature
    : IEntity, IEntitySoftDeletable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; init; }
    public bool IsActive { get; init; }
    public ushort Type { get; init; }
    public Guid PlanId { get; init; }
}