using Autofac;

using EasyCaching.Core;

using FooCommerce.Application.Commands.Notifications;
using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Entities.Membership;
using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Enums.Notifications;
using FooCommerce.Application.Models;
using FooCommerce.Application.Models.Notifications.Receivers;
using FooCommerce.Application.Services.Notifications;
using FooCommerce.NotificationAPI.CommandsHandlers;
using FooCommerce.NotificationAPI.Tests.Setups;
using FooCommerce.Tests.Base;

using MediatR;

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
        private IMediator MediatR { get; }
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
            MediatR = Scope.Resolve<IMediator>();
            NotificationTemplateService = Scope.Resolve<INotificationTemplateService>();
            Configuration = Scope.Resolve<IConfiguration>();
            Logger = Scope.Resolve<ILoggerFactory>();
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
                var userCommunication = dbContext.Set<UserCommunication>().Add(new UserCommunication(CommunicationType.Email_Message, "arash.shabbeh@gmail.com", user.Id)).Entity;
                await dbContext.SaveChangesAsync();
                userCommunicationId = userCommunication.Id;
            }

            var serviceProvider = Scope.Resolve<IServiceProvider>();
            var httpContextAccessor = serviceProvider.GetHttpContextAccessor();
            var httpContext = httpContextAccessor.HttpContext;

            var notification = new SendNotification(options =>
            {
                options.Action = NotificationAction.Verification_Request_Email;
                options.Receiver = new NotificationReceiverByCommunicationId(userCommunicationId);
                options.RequestInfo = new EndUser(httpContext);
            });
            var handler = new SendNotificationHandler(
                NotificationTemplateService,
                MediatR,
                Configuration,
                Logger
            );

            // Act
            await handler.Handle(notification, CancellationToken.None);

            // Assert
        }
    }
}