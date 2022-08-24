using Autofac;

using FooCommerce.Application.DbProvider;

namespace FooCommerce.Infrastructure.Modules;

public class DapperModule : Module
{
    private readonly string _connectionString;

    public DapperModule(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(ctx => new DbConnectionFactory(_connectionString))
            .As<IDbConnectionFactory>()
            .InstancePerDependency();
    }
}