using System.Data;

using Dapper;

using FooCommerce.Application.DbProvider.Interfaces;
using FooCommerce.Application.Membership.Entities;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Membership.Services;
using FooCommerce.Application.Notifications.Contracts;
using FooCommerce.Application.Notifications.Enums;
using FooCommerce.Application.Notifications.Models;
using FooCommerce.Domain.Enums;

using MassTransit;

namespace FooCommerce.Infrastructure.Membership;

public class VerificationService : IVerificationService
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IBus _bus;

    public VerificationService(IDbConnectionFactory dbConnectionFactory, IBus bus)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _bus = bus;
    }

    private static async Task<Guid?> CheckIfNotVerifiedYetAsync(CommunicationType type, string value, IDbConnection dbConnection, CancellationToken cancellationToken = default)
    {
        var nonVerifiedId = await dbConnection.QueryAsync<Guid>($"SELECT TOP(1) [communication].{nameof(UserCommunication.Id)} " +
                                                                 "FROM [UserCommunications] AS [communication] " +
                                                                 $"WHERE [communication].{nameof(UserCommunication.IsVerified)} = 0 AND [communication].{nameof(UserCommunication.Type)} = {(byte)type} AND [communication].{nameof(UserCommunication.Value)} = N'@Value'",
            new { Value = value.ToLowerInvariant() });
        if (nonVerifiedId == null || !nonVerifiedId.Any())
            return null;

        var establishedId = nonVerifiedId.Single();
        return establishedId != Guid.Empty ? establishedId : null;
    }

    private static string GenerateToken(CommunicationType type)
    {
        switch (type)
        {
            case CommunicationType.Email_Message:
                return Guid.NewGuid().ToString("N");

            case CommunicationType.Mobile_Sms:
                var instance = new Random();
                var num = instance.NextInt64(100_000, 999_999);
                return num.ToString();

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    private static async Task<AuthToken> CreateAuthTokenAsync(Guid communicationId, string token, IDbConnection dbConnection)
    {
        var authToken = await dbConnection.QuerySingleAsync<AuthToken>(
            $"INSERT INTO [AuthTokens] ([{nameof(AuthToken.Action)}], [{nameof(AuthToken.UserCommunicationId)}], [{nameof(AuthToken.Token)}]) OUTPUT INSERTED.* VALUES (@Action, @UserCommunicationId, @Token)", new
            {
                Action = (byte)AuthTokenAction.Request_EmailVerification,
                UserCommunicationId = communicationId,
                Token = token
            });
        return authToken;
    }

    public async Task<JobStatus> RequestVerificationAsync(CommunicationType type, string value, CancellationToken cancellationToken = default)
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();

        var communicationId = await CheckIfNotVerifiedYetAsync(type, value, dbConnection, cancellationToken);
        if (communicationId is not { })
            return JobStatus.Failed;

        var token = GenerateToken(type);
        var authToken = await CreateAuthTokenAsync(communicationId.Value, token, dbConnection);
        if (authToken == null)
            return JobStatus.Failed;

        await _bus.Publish<QueueNotification>(new
        {
            Action = type switch
            {
                CommunicationType.Email_Message => NotificationAction.Verification_Request_Email,
                CommunicationType.Mobile_Sms => NotificationAction.Verification_Request_Mobile,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            },
            Receiver = new NotificationReceiverProvider(NotificationReceiverStrategy.ByUserCommunicationId, communicationId.Value),
            Content = Enumerable.Range(0, 1).Select(_ => new NotificationFormatting("authToken", authToken.Token))
        }, cancellationToken);
        return JobStatus.Success;
    }
}