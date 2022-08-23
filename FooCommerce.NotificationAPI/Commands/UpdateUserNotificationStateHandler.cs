using Dapper;

using FooCommerce.Application.Commands.Notifications;
using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Entities.Messagings;
using FooCommerce.Application.Enums.Notifications;

using MediatR;

namespace FooCommerce.NotificationAPI.Commands;

public class UpdateUserNotificationStateHandler : INotificationHandler<UpdateUserNotificationState>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public UpdateUserNotificationStateHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task Handle(UpdateUserNotificationState notificationState, CancellationToken cancellationToken)
    {
        using (var dbConnection = _dbConnectionFactory.CreateConnection())
        {
            var columnName = notificationState.State switch
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
                    notificationState.UserNotificationId
                });
        }
    }
}