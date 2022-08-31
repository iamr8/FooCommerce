using System.Data;

using Dapper;

using FooCommerce.Application.Communications.Enums;
using FooCommerce.Application.DbProvider;
using FooCommerce.Domain.Enums;
using FooCommerce.MembershipAPI.Models;
using FooCommerce.MembershipAPI.Services;
using FooCommerce.MembershipAPI.Worker.DbProvider.Entities;
using FooCommerce.MembershipAPI.Worker.Enums;

using MassTransit;

namespace FooCommerce.MembershipAPI.Worker.Services;

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

    public async Task<RequestVerificationResponse> RequestVerificationAsync(CommunicationType type, string value, CancellationToken cancellationToken = default)
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();

        var communicationId = await CheckIfNotVerifiedYetAsync(type, value, dbConnection, cancellationToken);
        if (communicationId is not { })
            return JobStatus.Failed;

        var token = GenerateToken(type);
        var authToken = await CreateAuthTokenAsync(communicationId.Value, token, dbConnection);
        if (authToken == null)
            return JobStatus.Failed;

        return new RequestVerificationResponse(communicationId, authToken.Token);
    }
}