using System.Data;

using Autofac;

using EasyCaching.Core;

using FooCommerce.Common.Localization;
using FooCommerce.Domain.Enums;
using FooCommerce.MembershipAPI.Enums;
using FooCommerce.MembershipAPI.Worker.DbProvider;
using FooCommerce.MembershipAPI.Worker.DbProvider.Entities;
using FooCommerce.MembershipAPI.Worker.Enums;
using FooCommerce.MembershipAPI.Worker.Modules;
using FooCommerce.Tests;

using MassTransit.Testing;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, DisableTestParallelization = true, MaxParallelThreads = 1)]

namespace FooCommerce.MembershipAPI.Worker.Tests;

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
        containerBuilder.RegisterModule(new MembershipDatabaseProviderModule(connectionString, config =>
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

        var dbContextFactory = Container.Resolve<IDbContextFactory<MembershipDbContext>>();
        var dbContext = await dbContextFactory.CreateDbContextAsync();
        SeedMembershipData(dbContext);
        UserCommunicationId = await SeedCommunicationAsync(dbContext);
    }

    private async Task<Guid> SeedCommunicationAsync(MembershipDbContext dbContext)
    {
        var user = dbContext.Users.Add(new User()).Entity;
        var saved = await dbContext.SaveChangesAsync() > 0;
        var userInformation = dbContext.UserInformation.Add(new UserInformation
        {
            UserId = user.Id,
            Type = UserInformationType.Name,
            Value = "Arash"
        }).Entity;
        var userCommunication = dbContext.UserCommunications.Add(new UserCommunication
        {
            Type = CommunicationType.Email_Message,
            Value = "arash.shabbeh@gmail.com",
            IsVerified = true,
            UserId = user.Id,
        }).Entity;
        saved = await dbContext.SaveChangesAsync() > 0;
        return userCommunication.Id;
    }

    private void SeedMembershipData(MembershipDbContext dbContext)
    {
        var normalUserRole = dbContext.Roles.Add(new Role
        {
            Type = RoleType.NormalUser,
        }).Entity;
        var adminUserRole = dbContext.Roles.Add(new Role
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