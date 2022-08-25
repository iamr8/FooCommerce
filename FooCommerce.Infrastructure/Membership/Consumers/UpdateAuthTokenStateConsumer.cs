using Dapper;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Entities.Messagings;
using FooCommerce.Application.Enums.Notifications;
using FooCommerce.Application.Publishers.Membership;
using FooCommerce.Application.Publishers.Notifications;

using MassTransit;

using Microsoft.Extensions.Logging;

namespace FooCommerce.Infrastructure.Membership.Consumers;

public class UpdateAuthTokenStateConsumer : IConsumer<UpdateAuthTokenState>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<UpdateAuthTokenStateConsumer> _logger;

    public UpdateAuthTokenStateConsumer(IDbConnectionFactory dbConnectionFactory,
        ILogger<UpdateAuthTokenStateConsumer> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UpdateAuthTokenState> context)
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();
        const string query = $"SELECT TOP(1) [userNotification].{nameof(UserNotification.Id)} " +
                             $"FROM [UserNotifications] AS [userNotification] " +
                             $"WHERE [userNotification].{nameof(UserNotification.AuthTokenId)} = @AuthTokenId" +
                             $"ORDER BY [userNotification].{nameof(UserNotification.Created)} DESC";
        var notificationId = await dbConnection.QuerySingleAsync<Guid?>(query, new
        {
            context.Message.AuthTokenId
        });
        if (notificationId is not { })
        {
            _logger.LogError("Unable to save User Notification for Auth Token {0} as Sent.", context.Message.AuthTokenId);
            return;
        }

        await context.Publish(new UpdateUserNotificationState(notificationId.Value, UserNotificationUpdateState.Sent), context.CancellationToken);
    }
}