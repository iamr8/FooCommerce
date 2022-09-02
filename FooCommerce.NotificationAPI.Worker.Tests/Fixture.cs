using System.Data;

using Autofac;

using EasyCaching.Core;

using FooCommerce.Common.Localization;
using FooCommerce.Domain.Enums;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Worker.DbProvider;
using FooCommerce.NotificationAPI.Worker.DbProvider.Entities;
using FooCommerce.NotificationAPI.Worker.Modules;
using FooCommerce.NotificationAPI.Worker.Tests.Fakes.Entities;
using FooCommerce.NotificationAPI.Worker.Tests.Fakes.Enums;
using FooCommerce.Tests;

using MassTransit.Testing;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using RoleType = FooCommerce.NotificationAPI.Worker.Tests.Fakes.Enums.RoleType;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, DisableTestParallelization = true, MaxParallelThreads = 1)]

namespace FooCommerce.NotificationAPI.Worker.Tests;

public class Fixture : IAsyncLifetime, IFixture
{
    public IContainer Container { get; private set; }
    public ITestHarness Harness { get; private set; }
    private IConfigurationRoot Configuration;
    private string connectionString;
    public Guid UserCommunicationId;

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

        Harness = Container.Resolve<ITestHarness>();
        Harness.TestTimeout = TimeSpan.FromSeconds(2);

        await DatabaseCheckpoint.checkpoint.Reset(connectionString);

        var dbContextFactory = Container.Resolve<IDbContextFactory<AppDbContext>>();
        var dbContext = await dbContextFactory.CreateDbContextAsync();
        SeedMembershipData(dbContext);
        UserCommunicationId = await SeedCommunicationAsync(dbContext);
    }

    private async Task<Guid> SeedCommunicationAsync(DbContext dbContext)
    {
        var user = dbContext.Set<User>().Add(new User()).Entity;
        var saved = await dbContext.SaveChangesAsync() > 0;
        var userInformation = dbContext.Set<UserInformation>().Add(new UserInformation
        {
            UserId = user.Id,
            Type = UserInformationType.Name,
            Value = "Arash"
        }).Entity;
        var userCommunication = dbContext.Set<UserCommunication>().Add(new UserCommunication
        {
            Type = CommunicationType.Email_Message,
            Value = "arash.shabbeh@gmail.com",
            IsVerified = true,
            UserId = user.Id,
        }).Entity;
        var notification = dbContext.Set<Notification>().Add(new Notification
        {
            Action = NotificationAction.Verification_Request_Email,
        }).Entity;
        saved = await dbContext.SaveChangesAsync() > 0;
        var notificationTemplate = dbContext.Set<NotificationTemplate>().Add(new NotificationTemplate
        {
            NotificationId = notification.Id,
            IncludeRequest = true,
            JsonTemplate = "{\"h\":{\"en\":\"<p>123</p>\"}}",
            Type = CommunicationType.Email_Message
        }).Entity;
        saved = await dbContext.SaveChangesAsync() > 0;
        return userCommunication.Id;
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
        await DatabaseCheckpoint.checkpoint.Reset(connectionString);
        var cachingProvider = Container.Resolve<IEasyCachingProvider>();
        await cachingProvider.FlushAsync();
        GC.SuppressFinalize(this);
    }
}