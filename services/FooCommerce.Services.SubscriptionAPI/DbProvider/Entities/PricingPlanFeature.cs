using FooCommerce.Domain;

namespace FooCommerce.Services.SubscriptionAPI.DbProvider.Entities;

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