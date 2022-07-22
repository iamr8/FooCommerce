using FooCommerce.Domain.DbProvider.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FooCommerce.Domain.DbProvider
{
    public class AppDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public AppDbContext(DbContextOptions<AppDbContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assemblies = AppDomain.CurrentDomain.GetExecutingAssemblies().ToList();
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