using System.Globalization;

using FooCommerce.AspNetCoreExtensions.Helpers;
using FooCommerce.DbProvider;
using FooCommerce.DbProvider.Entities.Notifications;
using FooCommerce.Domain.ContextRequest;
using FooCommerce.Domain.Enums;
using FooCommerce.Localization;
using FooCommerce.Localization.Helpers;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Models;
using FooCommerce.NotificationAPI.Worker.Dtos;
using FooCommerce.NotificationAPI.Worker.Models;
using FooCommerce.NotificationAPI.Worker.Models.FactoryOptions;
using FooCommerce.NotificationAPI.Worker.Services;
using FooCommerce.NotificationAPI.Worker.Tests.Setup;
using FooCommerce.Tests;

using MassTransit;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

using Xunit.Abstractions;

namespace FooCommerce.NotificationAPI.Worker.Tests;

public class NotificationModelFactoryTests
{
    public ITestOutputHelper TestConsole { get; }

    public NotificationModelFactoryTests(ITestOutputHelper outputHelper)
    {
        TestConsole = outputHelper;
    }

    [Fact]
    public async Task Should_Return_Templates()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en");
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

        await using var fixture = new Fixture(TestConsole);
        await fixture.InitializeAsync();
        var templateService = fixture.ServiceProvider.GetService<INotificationTemplateService>();
        var dbContextFactory = fixture.ServiceProvider.GetService<IDbContextFactory<AppDbContext>>();
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
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
        }

        // Action
        var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        var notificationModel =
            await templateService.GetNotificationModelAsync(NotificationAction.Verification_Request_Email,
                tokenSource.Token);

        // Assert
        Assert.NotNull(notificationModel);
        Assert.NotEqual(Guid.Empty, notificationModel.NotificationId);
        Assert.NotEmpty(notificationModel.Templates);
        Assert.Single(notificationModel.Templates);
        Assert.Contains(notificationModel.Templates,
            template => template.Id != Guid.Empty &&
                        template.Communication == CommunicationType.Email_Message);
    }

    [Fact]
    public async Task Should_Return_TemplateFactory()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en");
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

        var userId = NewId.NextGuid();
        await using var fixture = new Fixture(TestConsole);
        await fixture.InitializeAsync();
        var dbContextFactory = fixture.ServiceProvider.GetService<IDbContextFactory<AppDbContext>>();
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
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
        }

        //var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        var serviceProvider = fixture.ServiceProvider.GetService<IServiceProvider>();
        var httpContextAccessor = serviceProvider.GetHttpContextAccessor();
        var httpContext = httpContextAccessor.HttpContext!;
        //var templates = await templateService.GetNotificationModelAsync(NotificationAction.Verification_Request_Email, tokenSource.Token);
        var notificationOptions = new NotificationOptions
        {
            Action = NotificationAction.Verification_Request_Email,
            ReceiverProvider = new NotificationReceiverProvider
            {
                Communications = new Dictionary<CommunicationType, string> { { CommunicationType.Email_Message, "arash.shabbeh@gmail.com" } },
                Name = "Arash",
                UserId = userId
            },
            RequestInfo = (ContextRequestInfo)httpContext.GetRequestInfo()
        };

        // Action

        var factory = NotificationModelFactory.CreateFactory(notificationOptions, fixture.ServiceProvider.GetService<ILoggerFactory>(), Mock.Of<ILocalizer>());

        // Assert
        Assert.NotNull(factory);
        Assert.IsType<NotificationModelFactory>(factory);
    }

    [Fact]
    public async Task Should_Return_Valid_EmailTemplate()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en");
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

        await using var fixture = new Fixture(TestConsole);
        await fixture.InitializeAsync();
        var templateService = fixture.ServiceProvider.GetService<INotificationTemplateService>();
        var dbContextFactory = fixture.ServiceProvider.GetService<IDbContextFactory<AppDbContext>>();
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
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
        }

        var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));

        // Action
        var notificationModel = await templateService.GetNotificationModelAsync(NotificationAction.Verification_Request_Email, tokenSource.Token);
        var emailTemplates = notificationModel.Templates.OfType<NotificationTemplateEmailModel>();

        // Assert
        Assert.NotNull(emailTemplates);
        Assert.NotEmpty(emailTemplates);
        Assert.Single(emailTemplates);

        var template = emailTemplates.First();
        Assert.NotNull(template);
        Assert.NotNull(template.Html);
        Assert.NotNull(template.Html.ToString());
        Assert.True(template.Html.ToString().Length > 0);
    }

    [Fact]
    public async Task Should_Create_EmailTemplate()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en");
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

        var userId = NewId.NextGuid();
        await using var fixture = new Fixture(TestConsole);
        await fixture.InitializeAsync();
        var templateService = fixture.ServiceProvider.GetService<INotificationTemplateService>();
        var dbContextFactory = fixture.ServiceProvider.GetService<IDbContextFactory<AppDbContext>>();
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
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
        }

        var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        var serviceProvider = fixture.ServiceProvider.GetService<IServiceProvider>();
        var httpContextAccessor = serviceProvider.GetHttpContextAccessor();
        var httpContext = httpContextAccessor.HttpContext!;
        var notificationModel = await templateService.GetNotificationModelAsync(NotificationAction.Verification_Request_Email, tokenSource.Token);
        var notificationOptions = new NotificationOptions
        {
            Action = NotificationAction.Verification_Request_Email,
            ReceiverProvider = new NotificationReceiverProvider
            {
                Communications = new Dictionary<CommunicationType, string> { { CommunicationType.Email_Message, "arash.shabbeh@gmail.com" } },
                Name = "Arash",
                UserId = userId
            },
            RequestInfo = (ContextRequestInfo)httpContext.GetRequestInfo()
        };
        var emailTemplate = notificationModel.Templates.OfType<NotificationTemplateEmailModel>().First();

        // Action
        var factory = NotificationModelFactory.CreateFactory(notificationOptions, fixture.ServiceProvider.GetService<ILoggerFactory>(), Mock.Of<ILocalizer>());
        var template = await factory.CreateEmailModelAsync(emailTemplate, new NotificationEmailModelFactoryOptions
        {
            LocalDateTime = DateTime.UtcNow.ToLocal(notificationOptions.RequestInfo),
            WebsiteUrl = "https://localhost:443",
        });

        // Assert
        Assert.NotNull(template);
        Assert.NotNull(template.Html);
        Assert.NotEmpty(template.Html);

        Assert.NotEmpty(template.Values);
        Assert.Contains(template.Values, value => value.Key == "{{request_ip}}" && value.Value == "78.173.224.233");
    }
}