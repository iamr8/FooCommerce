using Dapper;

using FooCommerce.Application.DbProvider;
using FooCommerce.NotificationAPI.Worker.Contracts;
using FooCommerce.NotificationAPI.Worker.DbProvider.Entities;
using FooCommerce.NotificationAPI.Worker.Enums;

using MassTransit;

namespace FooCommerce.NotificationAPI.Worker.Consumers;

public class UpdateUserNotificationStateConsumer
    : IConsumer<UpdateUserNotificationState>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public UpdateUserNotificationStateConsumer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task Consume(ConsumeContext<UpdateUserNotificationState> context)
    {
        using (var dbConnection = _dbConnectionFactory.CreateConnection())
        {
            var columnName = context.Message.State switch
            {
                UserNotificationState.Sent => nameof(UserNotification.Sent),
                UserNotificationState.Delivered => nameof(UserNotification.Delivered),
                UserNotificationState.Seen => nameof(UserNotification.Seen),
                _ => throw new ArgumentOutOfRangeException()
            };

            var userNotification = await dbConnection.QuerySingleAsync<UserNotification>(
                $"UPDATE [UserNotifications] SET {columnName} = GETUTCDATE() OUTPUT UPDATED.* WHERE Id = @UserNotificationId",
                new
                {
                    context.Message.UserNotificationId
                });
        }
    }
}