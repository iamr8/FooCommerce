using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Subscriptions.Entities.Configurations;

public class PricingPlanConfiguration : EntityConfiguration<PricingPlan>
{
    public override void Configure(EntityTypeBuilder<PricingPlan> builder)
    {
        builder.ToTable("PricingPlans");
        builder.HasOne(x => x.Category)
            .WithMany(x => x.Plans)
            .HasForeignKey(x => x.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}