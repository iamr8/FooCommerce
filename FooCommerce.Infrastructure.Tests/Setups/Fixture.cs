using System.Data;

using Autofac;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Entities.Listings;
using FooCommerce.Application.Entities.Membership;
using FooCommerce.Application.Enums.Membership;
using FooCommerce.Infrastructure.Caching;
using FooCommerce.Infrastructure.Locations;
using FooCommerce.Infrastructure.Modules;
using FooCommerce.Infrastructure.Shopping.StateMachines;
using FooCommerce.Tests.Base;

using MassTransit;
using MassTransit.Testing;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace FooCommerce.Infrastructure.Tests.Setups;

public class Fixture : IAsyncLifetime, IFixture
{
    public IContainer Container { get; private set; }
    public InMemoryTestHarness Harness;
    public IStateMachineSagaTestHarness<OrderState, OrderStateMachine> SagaHarness;
    public OrderStateMachine Machine;
    public IConfigurationRoot Configuration { get; set; }

    public async Task InitializeAsync()
    {
        // ConfigureServices
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .Build();
        var connectionString = Configuration.GetConnectionString("Default");

        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterModule(new AutoFluentValidationModule());
        containerBuilder.RegisterModule(new EventBusTestModule());
        containerBuilder.RegisterModule(new CachingModule());
        containerBuilder.RegisterModule(new DapperModule(connectionString));
        containerBuilder.RegisterModule(new DbContextModule(config =>
            //config.UseInMemoryDatabase(Guid.NewGuid().ToString(), b => b.EnableNullChecks(false))));
            config.UseSqlServer(connectionString, builder =>
            {
                builder.EnableRetryOnFailure(3);
            })));
        containerBuilder.RegisterType<SqlConnection>()
            .OnRelease(async ins => await ins.DisposeAsync())
            .As<IDbConnection>();

        // Configure
        Container = containerBuilder.Build();

        Harness = Container.Resolve<InMemoryTestHarness>();
        Harness.OnConfigureInMemoryBus += configurator => configurator.UseDelayedMessageScheduler();

        await Harness.Start();
        SagaHarness = Container.Resolve<IStateMachineSagaTestHarness<OrderState, OrderStateMachine>>();
        Machine = Container.Resolve<OrderStateMachine>();
        await DatabaseCheckpoint.checkpoint.Reset(connectionString);

        var dbContextFactory = Container.Resolve<IDbContextFactory<AppDbContext>>();
        var dbContext = await dbContextFactory.CreateDbContextAsync();
        AppDbContext.TestMode = true;
        SeedLocationsData(dbContext);
        SeedMembershipData(dbContext);
    }

    private void SeedLocationsData(DbContext dbContext)
    {
        var country = dbContext.Set<Location>().Add(new Location
        {
            Division = LocationDivision.Country,
            Name = "Iran",
        }).Entity;
        var province = dbContext.Set<Location>().Add(new Location
        {
            Division = LocationDivision.Province,
            Name = "Khuzestan",
            ParentId = country.Id,
        }).Entity;
        var city = dbContext.Set<Location>().Add(new Location
        {
            Division = LocationDivision.City,
            Name = "Ahvaz",
            ParentId = province.Id,
        }).Entity;
        var district = dbContext.Set<Location>().Add(new Location
        {
            Division = LocationDivision.District,
            Name = "Kianpars",
            ParentId = city.Id,
        }).Entity;
        var neighborhood = dbContext.Set<Location>().Add(new Location
        {
            Division = LocationDivision.Quarter,
            Name = "Western",
            ParentId = district.Id,
        }).Entity;
        dbContext.SaveChanges();
    }

    private void SeedMembershipData(DbContext dbContext)
    {
        var normalUserRole = dbContext.Set<Role>().Add(new Role
        {
            Type = RoleType.NormalUser,
        }).Entity;
        var adminUserRole = dbContext.Set<Role>().Add(new Role
        {
            Type = RoleType.Admin,
        }).Entity;
        dbContext.SaveChanges();
    }

    public async Task DisposeAsync()
    {
        var connectionString = Configuration.GetConnectionString("Default");
        await DatabaseCheckpoint.checkpoint.Reset(connectionString);
        var memoryCache = Container.Resolve<IMemoryCache>();
        memoryCache.Clear(LocationService.CacheKey);
        await Container.DisposeAsync();
    }
}