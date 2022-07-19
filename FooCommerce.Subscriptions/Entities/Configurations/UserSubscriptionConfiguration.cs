using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Subscriptions.Entities.Configurations;

public class UserSubscriptionConfiguration : EntityConfiguration<UserSubscription>
{
    public override void Configure(EntityTypeBuilder<UserSubscription> builder)
    {
        builder.ToTable("UserSubscriptions");
        builder.HasOne(x => x.Plan)
            .WithMany(x => x.UserSubscriptions)
            .HasForeignKey(x => x.PlanId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}