using System.Data;

using Autofac;

using EasyCaching.Core;

using FooCommerce.Common.Localization;
using FooCommerce.Domain.Enums;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Worker.DbProvider;
using FooCommerce.NotificationAPI.Worker.DbProvider.Entities;
using FooCommerce.NotificationAPI.Worker.Modules;
using FooCommerce.Tests;

using MassTransit;
using MassTransit.Testing;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, DisableTestParallelization = true, MaxParallelThreads = 1)]

namespace FooCommerce.NotificationAPI.Worker.Tests;

public class Fixture : IAsyncLifetime, IFixture
{
    public IContainer Container { get; private set; }
    public ITestHarness Harness { get; private set; }
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
        containerBuilder.RegisterModule(new LocalizationModule());
        containerBuilder.RegisterModule(new BusInMemoryTestModule());
        containerBuilder.RegisterModule(new CachingModule());
        containerBuilder.RegisterModule(new NotificationDatabaseProviderModule(connectionString, config =>
        // config.UseInMemoryDatabase(Guid.NewGuid().ToString(), config => config.EnableNullChecks(false))));
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

        Harness = Container.Resolve<ITestHarness>();
        Harness.TestTimeout = TimeSpan.FromSeconds(2);

        await DatabaseCheckpoint.checkpoint.Reset(connectionString);

        var dbContextFactory = Container.Resolve<IDbContextFactory<NotificationDbContext>>();
        var dbContext = await dbContextFactory.CreateDbContextAsync();
        await SeedNotificationsAsync(dbContext);
        NewId.NextGuid();
    }

    private async Task<Guid> SeedNotificationsAsync(NotificationDbContext dbContext)
    {
        var notification = dbContext.Notifications.Add(new Notification
        {
            Action = NotificationAction.Verification_Request_Email,
        }).Entity;
        await dbContext.SaveChangesAsync();
        var notificationTemplate = dbContext.NotificationTemplates.Add(new NotificationTemplate
        {
            NotificationId = notification.Id,
            IncludeRequest = true,
            JsonTemplate = "{\"h\":{\"en\":\"<p>123</p>\"}}",
            Type = CommunicationType.Email_Message
        }).Entity;
        await dbContext.SaveChangesAsync();
        return notification.Id;
    }

    public async Task DisposeAsync()
    {
        await DatabaseCheckpoint.checkpoint.Reset(connectionString);
        var cachingProvider = Container.Resolve<IEasyCachingProvider>();
        await cachingProvider.FlushAsync();
        GC.SuppressFinalize(this);
    }
}