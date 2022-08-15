using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Application.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Application.Modules;

public class DbContextModule : Module
{
    private readonly Action<DbContextOptionsBuilder<AppDbContext>> _dbContextOptionsBuilder;

    public DbContextModule(Action<DbContextOptionsBuilder<AppDbContext>> dbContextOptionsBuilder)
    {
        _dbContextOptionsBuilder = dbContextOptionsBuilder;
    }

    protected override void Load(ContainerBuilder builder)
    {
        if (builder.Properties.ContainsKey(GetType().AssemblyQualifiedName))
            return;

        builder.Properties.Add(GetType().AssemblyQualifiedName, null);

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