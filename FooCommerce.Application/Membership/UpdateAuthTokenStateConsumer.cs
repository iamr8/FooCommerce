//using Dapper;
//using FooCommerce.Core.DbProvider;
//using FooCommerce.NotificationAPI.Contracts;
//using FooCommerce.NotificationAPI.Entities;
//using FooCommerce.NotificationAPI.Enums;

//using MassTransit;

//using Microsoft.Extensions.Logging;

//namespace FooCommerce.NotificationAPI.Consumers;

//public class UpdateAuthTokenStateConsumer : IConsumer<UpdateAuthTokenState>
//{
//    private readonly IDbConnectionFactory _dbConnectionFactory;
//    private readonly ILogger<UpdateAuthTokenStateConsumer> _logger;

//    public UpdateAuthTokenStateConsumer(IDbConnectionFactory dbConnectionFactory,
//        ILogger<UpdateAuthTokenStateConsumer> logger)
//    {
//        _dbConnectionFactory = dbConnectionFactory;
//        _logger = logger;
//    }

//    public async Task Consume(ConsumeContext<UpdateAuthTokenState> context)
//    {
//        using var dbConnection = _dbConnectionFactory.CreateConnection();
//        const string query = $"SELECT TOP(1) [userNotification].{nameof(UserNotification.Id)} " +
//                             $"FROM [UserNotifications] AS [userNotification] " +
//                             $"WHERE [userNotification].{nameof(UserNotification.AuthTokenId)} = @AuthTokenId" +
//                             $"ORDER BY [userNotification].{nameof(UserNotification.Created)} DESC";
//        var notificationId = await dbConnection.QuerySingleAsync<Guid?>(query, new
//        {
//            context.Message.AuthTokenId
//        });
//        if (notificationId is not { })
//        {
//            _logger.LogError("Unable to save User Notification for Auth Token {0} as Sent.", context.Message.AuthTokenId);
//            return;
//        }

//        await context.Publish<UpdateUserNotificationState>(new
//        {
//            UserNotificationId = notificationId.Value,
//            State = UserNotificationUpdateState.Sent
//        }, context.CancellationToken);
//    }
//}