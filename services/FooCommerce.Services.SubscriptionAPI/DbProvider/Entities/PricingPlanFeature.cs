using FooCommerce.Domain;

namespace FooCommerce.SubscriptionsService.DbProvider.Entities;

public record PricingPlanFeature
    : IEntity, IEntitySoftDeletable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; init; }
    public ushort Type { get; init; }
    public Guid PlanId { get; init; }
}