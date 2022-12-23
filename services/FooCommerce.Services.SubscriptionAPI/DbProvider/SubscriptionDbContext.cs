using FooCommerce.SubscriptionsService.DbProvider.Entities;
using Microsoft.EntityFrameworkCore;

namespace FooCommerce.SubscriptionsService.DbProvider;

public class SubscriptionDbContext : DbContext
{
    public SubscriptionDbContext(DbContextOptions<SubscriptionDbContext> options) : base(options)
    {
        if (TestMode)
        {
            Database.EnsureCreated();
        }
    }

    public static bool TestMode { get; set; }

    public virtual DbSet<PricingPlan> PricingPlans { get; set; }
    public virtual DbSet<PricingPlanFeature> PricingPlanFeatures { get; set; }
    public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}