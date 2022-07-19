using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Subscriptions.Entities.Configurations;

public class PricingPlanCategoryConfiguration : EntityConfiguration<PricingPlanCategory>
{
    public override void Configure(EntityTypeBuilder<PricingPlanCategory> builder)
    {
        builder.ToTable("PricingPlanCategories");
    }
}