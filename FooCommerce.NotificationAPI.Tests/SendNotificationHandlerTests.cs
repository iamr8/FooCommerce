using Autofac;

using FooCommerce.Application;
using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Helpers;
using FooCommerce.Application.Membership.Entities;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Notifications.Contracts;
using FooCommerce.Application.Notifications.Enums;
using FooCommerce.Application.Notifications.Interfaces;
using FooCommerce.Application.Notifications.Models.Receivers;
using FooCommerce.NotificationAPI.Consumers;
using FooCommerce.NotificationAPI.Events;
using FooCommerce.NotificationAPI.Tests.Setups;
using FooCommerce.Tests.Base;

using MassTransit;
using MassTransit.Testing;

using Microsoft.EntityFrameworkCore;

using Xunit.Abstractions;

namespace FooCommerce.NotificationAPI.Tests
{
    public class SendNotificationHandlerTests : IClassFixture<Fixture>, ITestScope<Fixture>
    {
        public Fixture Fixture { get; }
        public ITestOutputHelper TestConsole { get; }
        public ILifetimeScope Scope { get; }

        // private IConfiguration Configuration { get; }
        // private ILoggerFactory Logger { get; }

        public SendNotificationHandlerTests(Fixture fixture, ITestOutputHelper outputHelper)
        {
            Fixture = fixture;
            TestConsole = outputHelper;
            Scope = fixture.ConfigureLogging(outputHelper);

            // var easyCaching = Scope.Resolve<IEasyCachingProvider>();
            // easyCaching.Flush();
            // var NotificationTemplateService = Scope.Resolve<INotificationTemplateService>();
            // Configuration = Scope.Resolve<IConfiguration>();
            // Logger = Scope.Resolve<ILoggerFactory>();
            //var machine = Scope.Resolve<NotificationStateMachine>();
            //var sagaHarness = Scope.Resolve<ISagaStateMachineTestHarness<NotificationStateMachine, NotificationState>>();
        }

        [Fact]
        public async Task Should_Fail_Message_Is_Null()
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
                saved = await dbContext.SaveChangesAsync() > 0;
                userCommunicationId = userCommunication.Id;
            }

            var serviceProvider = Scope.Resolve<IServiceProvider>();
            var httpContextAccessor = serviceProvider.GetHttpContextAccessor();
            var httpContext = httpContextAccessor.HttpContext!;
            var notificationId = NewId.NextGuid();

            await this.Fixture.Harness.Start();
            try
            {
                // Act
                var consumer = this.Fixture.Harness.GetConsumerHarness<QueueNotificationConsumer>();
                await this.Fixture.Harness.Bus.Publish<QueueNotification>(new
                {
                    NotificationId = notificationId,
                    Action = NotificationAction.Verification_Request_Email,
                    Receiver = (INotificationReceiver)new NotificationReceiverByCommunicationId(userCommunicationId),
                    RequestInfo = (IEndUser)httpContext.GetEndUser()
                });

                // Assert
                var published = await this.Fixture.Harness.Published.Any<QueueNotification>();
                Assert.True(published);

                var consumed = await consumer.Consumed.Any<QueueNotification>();
                Assert.True(consumed);

                var publishedFailure = await this.Fixture.Harness.Published.Any<NotificationFailed>();
                Assert.True(publishedFailure);
            }
            finally
            {
                await this.Fixture.Harness.Stop();
            }
        }

        [Fact]
        public async Task Should_Success()
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
                //var notification = dbContext.Set<Notification>().Add(new Notification
                //{
                //    Action = NotificationAction.Verification_Request_Email,
                //}).Entity;
                //saved = await dbContext.SaveChangesAsync() > 0;
                //var notificationTemplate = dbContext.Set<NotificationTemplate>().Add(new NotificationTemplate
                //{
                //    NotificationId = notification.Id,
                //    IncludeRequest = true,
                //    JsonTemplate = "{\"h\":\"<p>123</p>\"}",
                //    Type = CommunicationType.Email_Message
                //}).Entity;
                saved = await dbContext.SaveChangesAsync() > 0;
                userCommunicationId = userCommunication.Id;
            }

            var serviceProvider = Scope.Resolve<IServiceProvider>();
            var httpContextAccessor = serviceProvider.GetHttpContextAccessor();
            var httpContext = httpContextAccessor.HttpContext!;
            var notificationId = NewId.NextGuid();

            await this.Fixture.Harness.Start();
            try
            {
                // Act
                var consumer = this.Fixture.Harness.GetConsumerHarness<QueueNotificationConsumer>();
                //await this.Fixture.Harness.Bus.Publish<QueueNotification>(new
                //{
                //    NotificationId = notificationId,
                //    Options = (INotificationOptions)notificationOptions
                //});

                //var endpoint = await this.Fixture.Harness.Bus.GetSendEndpoint(new Uri("queue:queue-notification"));
                //await endpoint.Send<QueueNotification>(new
                //{
                //    NotificationId = notificationId,
                //    //Options = (INotificationOptions)notificationOptions
                //});

                var client = this.Fixture.Harness.GetRequestClient<QueueNotification>();
                var (acceptedTask, failedTask) = await client.GetResponse<NotificationQueued, NotificationFailed>(new
                {
                    NotificationId = notificationId,
                    Action = NotificationAction.Verification_Request_Email,
                    Receiver = (INotificationReceiver)new NotificationReceiverByCommunicationId(userCommunicationId),
                    RequestInfo = (IEndUser)httpContext.GetEndUser()
                });

                // Assert
                Assert.True(acceptedTask.IsCompletedSuccessfully);

                var accepted = await acceptedTask;
                Assert.NotEqual(Guid.Empty, accepted.Message.NotificationId);

                var published = await this.Fixture.Harness.Published.Any<QueueNotification>();
                Assert.True(published);

                var consumed = await consumer.Consumed.Any<QueueNotification>();
                Assert.True(consumed);

                var publishedSubmitted = await this.Fixture.Harness.Published.Any<NotificationQueued>();
                Assert.True(publishedSubmitted);
            }
            finally
            {
                await this.Fixture.Harness.Stop();
            }
        }
    }
}