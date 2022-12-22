﻿using FooCommerce.DbProvider.Entities.Configurations;
using FooCommerce.DbProvider.Entities.Listings.Entities;
using FooCommerce.DbProvider.Entities.Products;
using FooCommerce.DbProvider.Entities.Shoppings;
using FooCommerce.DbProvider.Entities.Subscriptions;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.DbProvider;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        if (TestMode)
        {
            Database.EnsureCreated();
        }
    }

    public static bool TestMode { get; set; }

    #region Configurations

    public virtual DbSet<Setting> Settings { get; set; }
    public virtual DbSet<Translation> Translations { get; set; }
    public virtual DbSet<Location> Locations { get; set; }

    #endregion Configurations

    #region Products

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductMultimedia> ProductMultimedia { get; set; }
    public virtual DbSet<ProductSpecification> ProductSpecifications { get; set; }
    public virtual DbSet<Specification> Specifications { get; set; }
    public virtual DbSet<TopCategory> TopCategories { get; set; }

    #endregion Products

    #region Shoppings

    public virtual DbSet<Checkout> Checkouts { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<ShoppingBasket> ShoppingBaskets { get; set; }
    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    #endregion Shoppings

    #region Subscriptions

    public virtual DbSet<PricingPlan> PricingPlans { get; set; }
    public virtual DbSet<PricingPlanFeature> PricingPlanFeatures { get; set; }
    public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }

    #endregion Subscriptions

    #region Listings

    public virtual DbSet<Listing> Listings { get; set; }
    public virtual DbSet<ListingComment> ListingComments { get; set; }
    public virtual DbSet<ListingLike> ListingLikes { get; set; }
    public virtual DbSet<ListingRating> ListingRatings { get; set; }
    public virtual DbSet<ListingReport> ListingReports { get; set; }
    public virtual DbSet<PurchasePrice> PurchasePrices { get; set; }

    #endregion Listings

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        if (TestMode)
        {
            modelBuilder.Entity<Location>(x => x.Ignore(c => c.Coordinate));
        }
    }
}