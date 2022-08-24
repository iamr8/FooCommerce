using Dapper;

using FooCommerce.Application.Commands.Membership;
using FooCommerce.Application.Commands.Notifications;
using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Entities.Messagings;
using FooCommerce.Application.Enums.Notifications;

using MediatR;

using Microsoft.Extensions.Logging;

namespace FooCommerce.Infrastructure.Membership.CommandHandlers;

public class UpdateAuthTokenStateHandler : INotificationHandler<UpdateAuthTokenState>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateAuthTokenStateHandler> _logger;

    public UpdateAuthTokenStateHandler(IDbConnectionFactory dbConnectionFactory, IMediator mediator, ILogger<UpdateAuthTokenStateHandler> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(UpdateAuthTokenState notificationState, CancellationToken cancellationToken)
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();
        const string query = $"SELECT TOP(1) [userNotification].{nameof(UserNotification.Id)} " +
                             $"FROM [UserNotifications] AS [userNotification] " +
                             $"WHERE [userNotification].{nameof(UserNotification.AuthTokenId)} = @AuthTokenId" +
                             $"ORDER BY [userNotification].{nameof(UserNotification.Created)} DESC";
        var notificationId = await dbConnection.QuerySingleAsync<Guid?>(query, new
        {
            notificationState.AuthTokenId
        });
        if (notificationId is not { })
        {
            _logger.LogError("Unable to save User Notification for Auth Token {0} as Sent.", notificationState.AuthTokenId);
            return;
        }

        await _mediator.Publish(new UpdateUserNotificationState(notificationId.Value, UserNotificationUpdateState.Sent), cancellationToken);
    }
}