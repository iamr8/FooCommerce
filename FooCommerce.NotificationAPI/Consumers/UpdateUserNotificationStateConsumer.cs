using Dapper;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Entities.Messagings;
using FooCommerce.Application.Enums.Notifications;
using FooCommerce.Application.Publishers.Notifications;

using MassTransit;

namespace FooCommerce.NotificationAPI.Consumers;

public class UpdateUserNotificationStateConsumer : IConsumer<UpdateUserNotificationState>
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
                UserNotificationUpdateState.Sent => nameof(UserNotification.Sent),
                UserNotificationUpdateState.Delivered => nameof(UserNotification.Delivered),
                UserNotificationUpdateState.Seen => nameof(UserNotification.Seen),
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