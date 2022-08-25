using System.Net;
using Autofac;

using EasyCaching.Core;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Entities.Membership;
using FooCommerce.Application.Entities.Messagings;
using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Enums.Notifications;
using FooCommerce.Application.Helpers;
using FooCommerce.Application.Models.Notifications;
using FooCommerce.Application.Models.Notifications.Receivers;
using FooCommerce.Application.Services.Notifications;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Tests.Setups;
using FooCommerce.Tests.Base;

using MassTransit;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace FooCommerce.NotificationAPI.Tests
{
    public class SendNotificationHandlerTests : IClassFixture<Fixture>, ITestScope<Fixture>
    {
        public Fixture Fixture { get; }
        public ITestOutputHelper TestConsole { get; }
        public ILifetimeScope Scope { get; }
        private INotificationTemplateService NotificationTemplateService { get; }
        private IConfiguration Configuration { get; }
        public ILoggerFactory Logger { get; }

        public SendNotificationHandlerTests(Fixture fixture, ITestOutputHelper outputHelper)
        {
            Fixture = fixture;
            TestConsole = outputHelper;
            Scope = fixture.ConfigureLogging(outputHelper);

            var easyCaching = Scope.Resolve<IEasyCachingProvider>();
            easyCaching.Flush();
            NotificationTemplateService = Scope.Resolve<INotificationTemplateService>();
            Configuration = Scope.Resolve<IConfiguration>();
            Logger = Scope.Resolve<ILoggerFactory>();
            //var machine = Scope.Resolve<NotificationStateMachine>();
            //var sagaHarness = Scope.Resolve<ISagaStateMachineTestHarness<NotificationStateMachine, NotificationState>>();
        }

        [Fact]
        public async Task Should_Send_Notification()
        {
            // Arrange
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
                    JsonTemplate = "{\"h\":\"<p>123</p>\"}",
                    Type = CommunicationType.Email_Message
                }).Entity;
                saved = await dbContext.SaveChangesAsync() > 0;
                userCommunicationId = userCommunication.Id;
            }

            var serviceProvider = Scope.Resolve<IServiceProvider>();
            var httpContextAccessor = serviceProvider.GetHttpContextAccessor();
            var httpContext = httpContextAccessor.HttpContext!;

            var options = new NotificationOptions
            {
                Action = NotificationAction.Verification_Request_Email,
                Receiver = new NotificationReceiverByCommunicationId(userCommunicationId),
                RequestInfo = httpContext.GetEndUser()
            };

            // Act
            var id = NewId.NextGuid();
            await Fixture.Harness.Bus.Publish<QueueNotification>(new
            {
                NotificationId = id,
                Options = options
            });

            await Fixture.Harness.Consumed.Any<QueueNotification>();
            //var client = Scope.Resolve<IRequestClient<GetNotification>>();

            //var response = await client.GetResponse<Order, OrderNotFound>(new { orderId });

            //// Assert
            //Assert.IsType<Order>(response.Message);
            //Assert.Equal("Submitted", ((Order)response.Message).Status);

            //Assert.IsNotType<Order>(response);
        }
    }
}