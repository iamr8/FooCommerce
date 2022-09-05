using FooCommerce.DbProvider;
using FooCommerce.DbProvider.Entities.Notifications;
using FooCommerce.NotificationAPI.Worker.Contracts;
using FooCommerce.NotificationAPI.Worker.Events;

using MassTransit;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.NotificationAPI.Worker.Consumers;

public class CreateUserNotificationConsumer
    : IConsumer<CreateUserNotification>
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly ILogger<CreateUserNotificationConsumer> _logger;

    public CreateUserNotificationConsumer(IDbContextFactory<AppDbContext> dbContextFactory, ILogger<CreateUserNotificationConsumer> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CreateUserNotification> context)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(context.CancellationToken);
        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async cancellationToken =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            {
                try
                {
                    var userNotification = dbContext.UserNotifications.Add(new UserNotification
                    {
                        UserId = context.Message.UserId,
                        NotificationId = context.Message.NotificationId,
                        RenderedContent = context.Message.Output,
                        Sent = context.Message.Sent
                    }).Entity;

                    await dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    await context.RespondAsync<UserNotificationCreationDone>(new
                    {
                        UserNotificationId = userNotification.Id
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    await transaction.RollbackAsync(cancellationToken);
                    await context.RespondAsync<UserNotificationCreationFaulted>(new
                    {
                        context.Message.Output,
                        context.Message.Sent,
                        context.Message.NotificationId,
                        context.Message.UserId,
                    });
                    throw;
                }
            }
        }, context.CancellationToken);
    }
}