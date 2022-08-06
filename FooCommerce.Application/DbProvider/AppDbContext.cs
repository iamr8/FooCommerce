using FooCommerce.Domain.Entities;

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
            var assemblies = AppDomain.CurrentDomain.GetExecutingAssemblies().ToList();
            foreach (var assembly in assemblies)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(assembly);

                var externalIdTypes = assembly.DefinedTypes
                    .Where(x => x.IsClass)
                    .Where(x => x.GetInterfaces().Any(c => c == typeof(IEntityPublicId)))
                    .ToList();
                if (externalIdTypes is { Count: > 0 })
                {
                    foreach (var type in externalIdTypes)
                    {
                        modelBuilder.HasSequence<long>(type.Name)
                            .StartsAt(1000)
                            .IncrementsBy(1);
                    }
                }
            }
        }
    }
}