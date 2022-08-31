using System.Data;

using Autofac;

using EasyCaching.Core;

using FooCommerce.Application.Listings.Entities;
using FooCommerce.Application.Localization.Enums;
using FooCommerce.Core.DbProvider;
using FooCommerce.Infrastructure.Modules;
using FooCommerce.Tests;

using MassTransit;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FooCommerce.Infrastructure.Tests.Setups;

public class Fixture : IAsyncLifetime, IFixture
{
    public IContainer Container { get; private set; }

    // public ITestHarness Harness;
    // public ISagaStateMachineTestHarness<OrderStateMachine, OrderState> SagaHarness;
    // public OrderStateMachine Machine;
    private IConfigurationRoot Configuration;

    private string connectionString;

    public async Task InitializeAsync()
    {
        // ConfigureServices
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables()
            .Build();
        connectionString = Configuration.GetConnectionString("Default");

        var containerBuilder = new ContainerBuilder();
        containerBuilder.Register(_ => MockObjects.GetWebHostEnvironment());
        containerBuilder.RegisterModule(new AutoFluentValidationModule());
        // containerBuilder.RegisterModule(new OrderAPIEventBusTestModule());
        containerBuilder.RegisterModule(new LocalizationModule());
        containerBuilder.RegisterModule(new CachingModule());
        containerBuilder.RegisterModule(new DatabaseProviderModule(connectionString, config =>
            config.UseSqlServer(connectionString!, builder =>
                builder.EnableRetryOnFailure(3)
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))));
        containerBuilder.RegisterType<SqlConnection>()
            .OnRelease(async ins => await ins.DisposeAsync())
            .As<IDbConnection>();
        containerBuilder.RegisterInstance(Configuration)
            .As<IConfiguration>()
            .SingleInstance();

        // Configure
        Container = containerBuilder.Build();

        // Harness = Container.Resolve<ITestHarness>();
        // Harness.TestTimeout = TimeSpan.FromSeconds(2);

        await DatabaseCheckpoint.checkpoint.Reset(connectionString);

        var dbContextFactory = Container.Resolve<IDbContextFactory<AppDbContext>>();
        var dbContext = await dbContextFactory.CreateDbContextAsync();
        AppDbContext.TestMode = true;
        SeedLocationsData(dbContext);
        // SagaHarness = Container.Resolve<ISagaStateMachineTestHarness<OrderStateMachine, OrderState>>();
        // Machine = Container.Resolve<OrderStateMachine>();
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

    public async Task DisposeAsync()
    {
        await DatabaseCheckpoint.checkpoint.Reset(connectionString);
        var cachingProvider = Container.Resolve<IEasyCachingProvider>();
        await cachingProvider.FlushAsync();
        GC.SuppressFinalize(this);
    }
}