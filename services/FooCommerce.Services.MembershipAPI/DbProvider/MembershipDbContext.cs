using FooCommerce.MembershipService.DbProvider.Entities;
using Microsoft.EntityFrameworkCore;

namespace FooCommerce.MembershipService.DbProvider;

public class MembershipDbContext : DbContext
{
    public MembershipDbContext(DbContextOptions<MembershipDbContext> options) : base(options)
    {
        if (TestMode)
        {
            Database.EnsureCreated();
        }
    }

    public static bool TestMode { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<UserRole> UserRoles { get; set; }
    public virtual DbSet<UserCommunication> UserCommunications { get; set; }
    public virtual DbSet<UserPassword> UserPasswords { get; set; }
    public virtual DbSet<UserSetting> UserSettings { get; set; }
    public virtual DbSet<UserLockout> UserLockouts { get; set; }
    public virtual DbSet<UserInformation> UserInformation { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}