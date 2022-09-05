using System.Globalization;

using FooCommerce.AspNetCoreExtensions.Helpers;
using FooCommerce.Domain.ContextRequest;
using FooCommerce.Domain.Enums;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Models;
using FooCommerce.NotificationAPI.Worker.Contracts;
using FooCommerce.NotificationAPI.Worker.Events;
using FooCommerce.NotificationAPI.Worker.Tests.Setup;
using FooCommerce.Tests;

using MassTransit;
using MassTransit.Testing;

using Microsoft.Extensions.DependencyInjection;

using Xunit.Abstractions;
using Xunit.Priority;

namespace FooCommerce.NotificationAPI.Worker.Tests;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class SendNotificationHandlerTests
{
    public ITestOutputHelper TestConsole { get; }

    public SendNotificationHandlerTests(ITestOutputHelper outputHelper)
    {
        TestConsole = outputHelper;
    }

    [Fact, Priority(1)]
    public async Task Should_Queue_InAllWays()
    {
        // Arrange
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en");
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

        await using var fixture = new Fixture(TestConsole);
        await fixture.InitializeAsync();
        var serviceProvider = fixture.ServiceProvider.GetService<IServiceProvider>();
        var httpContextAccessor = serviceProvider.GetHttpContextAccessor();
        var httpContext = httpContextAccessor.HttpContext!;

        // Act
        await fixture.Harness.Start();
        try
        {
            // var consumerQueueNotification = fixture.Harness.GetConsumerHarness<QueueNotificationConsumer>();
            // var consumerQueueNotificationEmail = fixture.Harness.GetConsumerHarness<QueueNotificationEmailConsumer>();
            // var consumerCreateUserNotification = fixture.Harness.GetConsumerHarness<CreateUserNotificationConsumer>();

            await fixture.Harness.Bus.Publish<QueueNotification>(new
            {
                fixture.UserId,
                Action = NotificationAction.Verification_Request_Email,
                ReceiverProvider = new NotificationReceiverProvider
                {
                    Communications = new Dictionary<CommunicationType, string> { { CommunicationType.Email_Message, "arash.shabbeh@gmail.com" } },
                    Name = "Arash",
                    UserId = fixture.UserId
                },
                RequestInfo = (ContextRequestInfo)httpContext.GetRequestInfo()
            });

            // Assert
            var published = await fixture.Harness.Published.Any<QueueNotification>();
            Assert.True(published);

            var publishedEmail = await fixture.Harness.Published.Any<QueueNotificationEmail>();
            Assert.True(publishedEmail);

            var notificationSent = await fixture.Harness.Published.Any<NotificationSent>();
            Assert.True(notificationSent);

            var consumed = await fixture.Harness.Consumed.Any<QueueNotification>();
            Assert.True(consumed);

            var consumedEmail = await fixture.Harness.Consumed.Any<QueueNotificationEmail>();
            Assert.True(consumedEmail);

            var consumedUpdateNotification = await fixture.Harness.Consumed.Any<CreateUserNotification>();
            Assert.True(consumedUpdateNotification);

            var publishedUpdateNotificationDone = await fixture.Harness.Published.Any<UserNotificationCreationDone>();
            Assert.True(publishedUpdateNotificationDone);
        }
        finally
        {
            await fixture.Harness.Stop();
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
    //    await fixture.Harness.Start();
    //    try
    //    {
    //        var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
    //        var consumer = fixture.Harness.GetConsumerHarness<QueueNotificationConsumer>();

    //        var client = fixture.Harness.GetRequestClient<QueueNotification>();
    //        var response = await client.GetResponse<NotificationSent>(new
    //        {
    //            NotificationId = notificationId,
    //            Action = NotificationAction.Verification_Request_Email,
    //            Receiver = new NotificationReceiverProvider(NotificationReceiverStrategy.ByUserCommunicationId, fixture.UserCommunicationId),
    //            RequestInfo = HttpContext.GetRequestInfo()
    //        }, cancellationTokenSource.Token);

    //        Assert.NotEqual(Guid.Empty, response.Message.NotificationId);

    //        var published = await fixture.Harness.Published.Any<QueueNotification>(cancellationTokenSource.Token);
    //        Assert.True(published);

    //        var consumed = await consumer.Consumed.Any<QueueNotification>(cancellationTokenSource.Token);
    //        Assert.True(consumed);

    //        var publishedSent = await fixture.Harness.Published.Any<NotificationSent>(cancellationTokenSource.Token);
    //        Assert.True(publishedSent);
    //    }
    //    finally
    //    {
    //        await fixture.Harness.Stop();
    //    }
    //}
}