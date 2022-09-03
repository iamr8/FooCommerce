using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Domain.DbProvider;
using FooCommerce.MembershipAPI.Worker.DbProvider;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.MembershipAPI.Worker.Modules;

public class MembershipDatabaseProviderModule : Module
{
    private readonly Action<DbContextOptionsBuilder<MembershipDbContext>> _dbContextOptionsBuilder;
    private readonly string _connectionString;

    public MembershipDatabaseProviderModule(string connectionString, Action<DbContextOptionsBuilder<MembershipDbContext>> dbContextOptionsBuilder)
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
        serviceCollection.AddDbContextFactory<MembershipDbContext>(options =>
        {
            _dbContextOptionsBuilder((DbContextOptionsBuilder<MembershipDbContext>)options);
            options
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .EnableThreadSafetyChecks();
        });

        builder.Populate(serviceCollection);
    }
}