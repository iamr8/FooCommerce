using FooCommerce.Application.Entities.Listings;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Application.DbProvider;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        if (TestMode)
        {
            this.Database.EnsureCreated();
        }
    }

    public static bool TestMode { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        if (TestMode)
        {
            modelBuilder.Entity<Location>(x => x.Ignore(c => c.Coordinate));
        }
    }
}