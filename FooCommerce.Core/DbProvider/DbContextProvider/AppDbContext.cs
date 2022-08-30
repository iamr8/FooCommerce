using FooCommerce.Application.Helpers;
using FooCommerce.Application.Listings.Entities;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Core.DbProvider.DbContextProvider;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assemblies = AppDomain.CurrentDomain.GetExecutingAssemblies();
        foreach (var assembly in assemblies)
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        if (TestMode)
        {
            modelBuilder.Entity<Location>(x => x.Ignore(c => c.Coordinate));
        }
    }
}