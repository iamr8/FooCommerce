using System.Globalization;

using Autofac;

using FooCommerce.Application.Communications.Enums;
using FooCommerce.Application.Membership.Entities;
using FooCommerce.Core.DbProvider;
using FooCommerce.Core.Helpers;
using FooCommerce.Core.HttpContextRequest;
using FooCommerce.Domain;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Models;
using FooCommerce.NotificationAPI.Services;
using FooCommerce.NotificationAPI.Worker.DbProvider.Entities;
using FooCommerce.NotificationAPI.Worker.Dtos;
using FooCommerce.NotificationAPI.Worker.Models;
using FooCommerce.NotificationAPI.Worker.Models.FactoryOptions;
using FooCommerce.NotificationAPI.Worker.Tests.Fakes;
using FooCommerce.Tests;
using FooCommerce.Tests.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Moq;

using Xunit.Abstractions;

namespace FooCommerce.NotificationAPI.Worker.Tests
{
    public class NotificationModelFactoryTests : IClassFixture<Fixture>, ITestScope<Fixture>
    {
        public Fixture Fixture { get; }
        public ITestOutputHelper TestConsole { get; }
        public ILifetimeScope Scope { get; }
        public INotificationTemplateService TemplateService { get; }

        public NotificationModelFactoryTests(Fixture fixture, ITestOutputHelper outputHelper)
        {
            Fixture = fixture;
            TestConsole = outputHelper;
            Scope = fixture.ConfigureLogging(outputHelper);
            TemplateService = Scope.Resolve<INotificationTemplateService>();
        }

        [Fact]
        public async Task Should_Return_Templates()
        {
            // Arrange
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

            var dbContextFactory = Scope.Resolve<IDbContextFactory<AppDbContext>>();
            await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
            {
                var user = dbContext.Set<User>().Add(new User()).Entity;
                var saved = await dbContext.SaveChangesAsync() > 0;
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
            }

            // Action
            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));
            var templates =
                await TemplateService.GetTemplateAsync(NotificationAction.Verification_Request_Email,
                    tokenSource.Token);

            // Assert
            Assert.NotNull(templates);
            Assert.NotEmpty(templates);
            Assert.Single(templates);
        }

        [Fact]
        public async Task Should_Return_TemplateFactory()
        {
            // Arrange
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

            Guid userCommunicationId;
            var dbContextFactory = Scope.Resolve<IDbContextFactory<AppDbContext>>();
            await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
            {
                var user = dbContext.Set<User>().Add(new User()).Entity;
                var saved = await dbContext.SaveChangesAsync() > 0;
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
                userCommunicationId = userCommunication.Id;
            }

            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));
            var serviceProvider = Scope.Resolve<IServiceProvider>();
            var httpContextAccessor = serviceProvider.GetHttpContextAccessor();
            var httpContext = httpContextAccessor.HttpContext!;
            var templates =
                await TemplateService.GetTemplateAsync(NotificationAction.Verification_Request_Email,
                    tokenSource.Token);
            var notificationOptions = new NotificationOptions
            {
                Action = NotificationAction.Verification_Request_Email,
                Receiver = new NotificationReceiverProvider(NotificationReceiverStrategy.ByUserCommunicationId, userCommunicationId),
                RequestInfo = (HttpRequestInfo)httpContext.GetRequestInfo()
            };

            // Action

            var factory = NotificationModelFactory.CreateFactory(notificationOptions, Scope.Resolve<ILoggerFactory>(), Mock.Of<ILocalizer>());

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

            Guid userCommunicationId;
            Guid templateId;
            var dbContextFactory = Scope.Resolve<IDbContextFactory<AppDbContext>>();
            await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
            {
                var user = dbContext.Set<User>().Add(new User()).Entity;
                var saved = await dbContext.SaveChangesAsync() > 0;
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
                userCommunicationId = userCommunication.Id;
                templateId = notificationTemplate.Id;
            }

            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));

            // Action
            var templates = await TemplateService.GetTemplateAsync(NotificationAction.Verification_Request_Email, tokenSource.Token);
            var emailTemplates = templates.OfType<NotificationTemplateEmailModel>();

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

            Guid userCommunicationId;
            Guid templateId;
            var dbContextFactory = Scope.Resolve<IDbContextFactory<AppDbContext>>();
            await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
            {
                var user = dbContext.Set<User>().Add(new User()).Entity;
                var saved = await dbContext.SaveChangesAsync() > 0;
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
                userCommunicationId = userCommunication.Id;
                templateId = notificationTemplate.Id;
            }

            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));
            var serviceProvider = Scope.Resolve<IServiceProvider>();
            var httpContextAccessor = serviceProvider.GetHttpContextAccessor();
            var httpContext = httpContextAccessor.HttpContext!;
            var templates = await TemplateService.GetTemplateAsync(NotificationAction.Verification_Request_Email, tokenSource.Token);
            var notificationOptions = new NotificationOptions
            {
                Action = NotificationAction.Verification_Request_Email,
                Receiver = new NotificationReceiverProvider(NotificationReceiverStrategy.ByUserCommunicationId, userCommunicationId),
                RequestInfo = (HttpRequestInfo)httpContext.GetRequestInfo()
            };
            var emailTemplate = templates.OfType<NotificationTemplateEmailModel>().First();

            // Action
            var factory = NotificationModelFactory.CreateFactory(notificationOptions, Scope.Resolve<ILoggerFactory>(), Mock.Of<ILocalizer>());
            var template = await factory.CreateEmailModelAsync(emailTemplate, new NotificationEmailModelFactoryOptions
            {
                LocalDateTime = DateTime.UtcNow.ToLocal(notificationOptions.RequestInfo),
                WebsiteUrl = "https://localhost:443",
            });

            // Assert
            Assert.NotNull(template);
            Assert.NotNull(template.Html);
            Assert.NotEmpty(template.Html.ToString());

            Assert.NotEmpty(template.Values);
            Assert.Contains(template.Values, value => value.Key == "{{request_ip}}" && value.Value == "78.173.224.233");
        }
    }
}