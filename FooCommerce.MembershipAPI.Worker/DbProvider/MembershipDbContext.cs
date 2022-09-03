using FooCommerce.Common.Helpers;
using FooCommerce.MembershipAPI.Worker.DbProvider.Entities;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.MembershipAPI.Worker.DbProvider;

public class MembershipDbContext : DbContext
{
    public MembershipDbContext(DbContextOptions<MembershipDbContext> options) : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<UserRole> UserRoles { get; set; }
    public virtual DbSet<UserCommunication> UserCommunications { get; set; }
    public virtual DbSet<UserPassword> UserPasswords { get; set; }
    public virtual DbSet<UserSetting> UserSettings { get; set; }
    public virtual DbSet<UserLockout> UserLockouts { get; set; }
    public virtual DbSet<UserInformation> UserInformation { get; set; }
    public virtual DbSet<AuthToken> AuthTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assemblies = AppDomain.CurrentDomain.GetExecutingAssemblies();
        foreach (var assembly in assemblies)
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }
}