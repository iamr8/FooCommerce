using FooCommerce.Services.BasketAPI.DbProvider.Entities;
using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Services.BasketAPI.DbProvider;

public class BasketDbContext : DbContext
{
    public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options)
    {
        if (TestMode)
        {
            Database.EnsureCreated();
        }
    }

    public static bool TestMode { get; set; }

    public virtual DbSet<ShoppingBasket> ShoppingBaskets { get; set; }
    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}