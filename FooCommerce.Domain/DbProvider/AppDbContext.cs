using FooCommerce.Domain.DbProvider.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Domain.DbProvider
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.GetName().Name.StartsWith(nameof(FooCommerce)))
                .Where(x => !x.GetName().Name.EndsWith(".Tests"))
                .ToList();
            foreach (var assembly in assemblies)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(assembly);

                var externalIdTypes = assembly.DefinedTypes
                    .Where(x => x.IsClass)
                    .Where(x => x.GetInterfaces().Any(c => c == typeof(IEntityExternalId)))
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