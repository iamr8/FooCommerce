using FooCommerce.Common.Configurations;
using FooCommerce.DbProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Bootstrapper.Modules;

public class AppDatabaseProviderModule : Module
{
    private readonly Action<DbContextOptionsBuilder<AppDbContext>> _dbContextOptionsBuilder;
    private readonly string _connectionString;

    public AppDatabaseProviderModule(string connectionString, Action<DbContextOptionsBuilder<AppDbContext>> dbContextOptionsBuilder)
    {
        _dbContextOptionsBuilder = dbContextOptionsBuilder;
        _connectionString = connectionString;
    }

    public void Load(IServiceCollection services)
    {
        services.AddTransient<IDbConnectionFactory>(_ => new DbConnectionFactory(_connectionString));
        services.AddTransient(sp => sp.GetService<IDbConnectionFactory>().CreateConnection());
        services.AddDbContextFactory<AppDbContext>(options =>
        {
            _dbContextOptionsBuilder((DbContextOptionsBuilder<AppDbContext>)options);
            options
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .EnableThreadSafetyChecks()
                .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
        });
    }
}