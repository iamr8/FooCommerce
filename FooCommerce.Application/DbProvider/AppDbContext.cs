using FooCommerce.Application.Entities.Listings;
using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Application.DbProvider
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Listing>();
            var assemblies = AppDomain.CurrentDomain.GetExecutingAssemblies().ToList();
            foreach (var assembly in assemblies)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            }
        }
    }
}