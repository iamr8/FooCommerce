using System.Globalization;

using Autofac;

using FooCommerce.Common.Helpers;
using FooCommerce.Common.HttpContextRequest;
using FooCommerce.Domain.Enums;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Models;
using FooCommerce.NotificationAPI.Worker.Contracts;
using FooCommerce.NotificationAPI.Worker.Events;
using FooCommerce.Tests;
using FooCommerce.Tests.Extensions;

using MassTransit;
using MassTransit.Testing;

using Microsoft.AspNetCore.Http;

using Xunit.Abstractions;
using Xunit.Priority;

namespace FooCommerce.NotificationAPI.Worker.Tests;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class SendNotificationHandlerTests : IClassFixture<Fixture>, ITestScope<Fixture>
{
    public Fixture Fixture { get; }
    public ITestOutputHelper TestConsole { get; }
    public ILifetimeScope Scope { get; }
    public HttpContext HttpContext { get; }

    public SendNotificationHandlerTests(Fixture fixture, ITestOutputHelper outputHelper)
    {
        Fixture = fixture;
        TestConsole = outputHelper;
        Scope = fixture.ConfigureLogging(outputHelper);

        var serviceProvider = Scope.Resolve<IServiceProvider>();
        var httpContextAccessor = serviceProvider.GetHttpContextAccessor();
        HttpContext = httpContextAccessor.HttpContext!;
    }

    [Fact, Priority(1)]
    public async Task Should_Queue_InAllWays()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en");
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

        // Act
        await this.Fixture.Harness.Start();
        try
        {
            // var consumerQueueNotification = this.Fixture.Harness.GetConsumerHarness<QueueNotificationConsumer>();
            // var consumerQueueNotificationEmail = this.Fixture.Harness.GetConsumerHarness<QueueNotificationEmailConsumer>();
            // var consumerCreateUserNotification = this.Fixture.Harness.GetConsumerHarness<CreateUserNotificationConsumer>();

            await this.Fixture.Harness.Bus.Publish<QueueNotification>(new
            {
                UserId = NewId.NextGuid(),
                Action = NotificationAction.Verification_Request_Email,
                ReceiverProvider = new NotificationReceiverProvider
                {
                    Communications = new Dictionary<CommunicationType, string> { { CommunicationType.Email_Message, "arash.shabbeh@gmail.com" } },
                    Name = "Arash",
                    UserId = NewId.NextGuid()
                },
                RequestInfo = (HttpRequestInfo)HttpContext.GetRequestInfo()
            });

            // Assert
            var published = await this.Fixture.Harness.Published.Any<QueueNotification>();
            Assert.True(published);

            var publishedEmail = await this.Fixture.Harness.Published.Any<QueueNotificationEmail>();
            Assert.True(publishedEmail);

            var notificationSent = await this.Fixture.Harness.Published.Any<NotificationSent>();
            Assert.True(notificationSent);

            var consumed = await this.Fixture.Harness.Consumed.Any<QueueNotification>();
            Assert.True(consumed);

            var consumedEmail = await this.Fixture.Harness.Consumed.Any<QueueNotificationEmail>();
            Assert.True(consumedEmail);

            var consumedUpdateNotification = await this.Fixture.Harness.Consumed.Any<CreateUserNotification>();
            Assert.True(consumedUpdateNotification);

            var publishedUpdateNotificationDone = await this.Fixture.Harness.Published.Any<UserNotificationCreationDone>();
            Assert.True(publishedUpdateNotificationDone);
        }
        finally
        {
            await this.Fixture.Harness.Stop();
        }
    }

    //[Fact, Priority(2)]
    //public async Task Should_Send_And_Responded_NotificationQueued()
    //{
    //    // Arrange
    //    CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en");
    //    CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");
    //    var notificationId = NewId.NextGuid();

    //    // Act
    //    await this.Fixture.Harness.Start();
    //    try
    //    {
    //        var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
    //        var consumer = this.Fixture.Harness.GetConsumerHarness<QueueNotificationConsumer>();

    //        var client = this.Fixture.Harness.GetRequestClient<QueueNotification>();
    //        var response = await client.GetResponse<NotificationSent>(new
    //        {
    //            NotificationId = notificationId,
    //            Action = NotificationAction.Verification_Request_Email,
    //            Receiver = new NotificationReceiverProvider(NotificationReceiverStrategy.ByUserCommunicationId, this.Fixture.UserCommunicationId),
    //            RequestInfo = HttpContext.GetRequestInfo()
    //        }, cancellationTokenSource.Token);

    //        Assert.NotEqual(Guid.Empty, response.Message.NotificationId);

    //        var published = await this.Fixture.Harness.Published.Any<QueueNotification>(cancellationTokenSource.Token);
    //        Assert.True(published);

    //        var consumed = await consumer.Consumed.Any<QueueNotification>(cancellationTokenSource.Token);
    //        Assert.True(consumed);

    //        var publishedSent = await this.Fixture.Harness.Published.Any<NotificationSent>(cancellationTokenSource.Token);
    //        Assert.True(publishedSent);
    //    }
    //    finally
    //    {
    //        await this.Fixture.Harness.Stop();
    //    }
    //}
}