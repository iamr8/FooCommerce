using FooCommerce.Services.NotificationAPI.DbProvider.Entities;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Services.NotificationAPI.DbProvider;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
    {
        if (TestMode)
        {
            Database.EnsureCreated();
        }
    }

    public static bool TestMode { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<NotificationTemplate> NotificationTemplates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}