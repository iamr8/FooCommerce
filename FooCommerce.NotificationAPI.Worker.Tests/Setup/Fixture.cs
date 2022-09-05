using EasyCaching.Core;

using FooCommerce.DbProvider;
using FooCommerce.DbProvider.Entities.Identities;
using FooCommerce.DbProvider.Entities.Notifications;
using FooCommerce.Domain.Enums;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.Tests;

using MassTransit;
using MassTransit.Testing;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Xunit.Abstractions;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, DisableTestParallelization = true, MaxParallelThreads = 1)]

namespace FooCommerce.NotificationAPI.Worker.Tests.Setup;

public class Fixture : IFixture
{
    public IServiceProvider ServiceProvider { get; private set; }
    public ITestHarness Harness { get; private set; }
    private readonly ITestOutputHelper _output;
    private string connectionString;
    public Guid UserId { get; private set; }
    public Guid UserCommunicationId { get; private set; }

    public Fixture(ITestOutputHelper output)
    {
        _output = output;
    }

    public async Task InitializeAsync()
    {
        // ConfigureServices
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables()
            .Build();
        connectionString = configuration.GetConnectionString("Default");

        var testStartup = new TestStartup(configuration, connectionString, _output);
        var services = new ServiceCollection();

        // Configure
        ServiceProvider = testStartup.ConfigureServices(services);

        LogContext.ConfigureCurrentLogContextIfNull(ServiceProvider);
        Harness = ServiceProvider.GetTestHarness();
        Harness.TestTimeout = TimeSpan.FromSeconds(2);

        await DatabaseCheckpoint.checkpoint.Reset(connectionString);

        var dbContextFactory = ServiceProvider.GetService<IDbContextFactory<AppDbContext>>();
        var dbContext = await dbContextFactory.CreateDbContextAsync();
        SeedMembershipData(dbContext);
        await SeedCommunicationAsync(dbContext);
        await SeedNotificationsAsync(dbContext);
    }

    private async Task SeedCommunicationAsync(AppDbContext dbContext)
    {
        var user = dbContext.Users.Add(new User()).Entity;
        var saved = await dbContext.SaveChangesAsync() > 0;
        var userInformation = dbContext.UserInformation.Add(new UserInformation
        {
            UserId = user.Id,
            Type = 0,
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
        UserId = user.Id;
        UserCommunicationId = userCommunication.Id;
    }

    private void SeedMembershipData(AppDbContext dbContext)
    {
        var normalUserRole = dbContext.Roles.Add(new Role
        {
            Type = 0,
        }).Entity;
        var adminUserRole = dbContext.Roles.Add(new Role
        {
            Type = 100,
        }).Entity;
        dbContext.SaveChanges();
    }

    private async Task<Guid> SeedNotificationsAsync(AppDbContext dbContext)
    {
        var notification = dbContext.Notifications.Add(new Notification
        {
            Action = (short)NotificationAction.Verification_Request_Email,
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

    public async ValueTask DisposeAsync()
    {
        await DatabaseCheckpoint.checkpoint.Reset(connectionString);
        var cachingProvider = ServiceProvider.GetService<IEasyCachingProvider>();
        await cachingProvider.FlushAsync();
        GC.SuppressFinalize(this);
    }
}