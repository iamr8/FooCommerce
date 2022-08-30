using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Core.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Modules;

public class DatabaseProviderModule : Module
{
    private readonly Action<DbContextOptionsBuilder<AppDbContext>> _dbContextOptionsBuilder;
    private readonly string _connectionString;

    public DatabaseProviderModule(string connectionString, Action<DbContextOptionsBuilder<AppDbContext>> dbContextOptionsBuilder)
    {
        _dbContextOptionsBuilder = dbContextOptionsBuilder;
        _connectionString = connectionString;
    }

    protected override void Load(ContainerBuilder builder)
    {
        if (builder.Properties.ContainsKey(GetType().AssemblyQualifiedName))
            return;

        builder.Properties.Add(GetType().AssemblyQualifiedName, null);

        builder.Register(ctx => new DbConnectionFactory(_connectionString))
            .As<IDbConnectionFactory>()
            .InstancePerDependency();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContextFactory<AppDbContext>(options =>
        {
            _dbContextOptionsBuilder((DbContextOptionsBuilder<AppDbContext>)options);
            options
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .EnableThreadSafetyChecks()
                .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
        });

        builder.Populate(serviceCollection);
    }
}