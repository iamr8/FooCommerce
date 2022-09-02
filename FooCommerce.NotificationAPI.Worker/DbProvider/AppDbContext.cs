using FooCommerce.Common.Helpers;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.NotificationAPI.Worker.DbProvider;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assemblies = AppDomain.CurrentDomain.GetExecutingAssemblies();
        foreach (var assembly in assemblies)
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }
}