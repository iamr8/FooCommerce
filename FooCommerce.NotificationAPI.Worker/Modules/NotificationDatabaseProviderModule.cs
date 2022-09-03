using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Domain.DbProvider;
using FooCommerce.NotificationAPI.Worker.DbProvider;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.NotificationAPI.Worker.Modules;

public class NotificationDatabaseProviderModule : Module
{
    private readonly Action<DbContextOptionsBuilder<NotificationDbContext>> _dbContextOptionsBuilder;
    private readonly string _connectionString;

    public NotificationDatabaseProviderModule(string connectionString, Action<DbContextOptionsBuilder<NotificationDbContext>> dbContextOptionsBuilder)
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
        serviceCollection.AddDbContextFactory<NotificationDbContext>(options =>
        {
            _dbContextOptionsBuilder((DbContextOptionsBuilder<NotificationDbContext>)options);
            options
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .EnableThreadSafetyChecks();
        });

        builder.Populate(serviceCollection);
    }
}