using FooCommerce.Services.ProductAPI.DbProvider.Entities.Listings.Entities;
using FooCommerce.Services.ProductAPI.DbProvider.Entities.Products;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Services.ProductAPI.DbProvider;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
        if (TestMode)
        {
            Database.EnsureCreated();
        }
    }

    public static bool TestMode { get; set; }

    #region Products

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductMultimedia> ProductMultimedia { get; set; }
    public virtual DbSet<ProductSpecification> ProductSpecifications { get; set; }
    public virtual DbSet<Specification> Specifications { get; set; }
    public virtual DbSet<TopCategory> TopCategories { get; set; }

    #endregion Products

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
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}