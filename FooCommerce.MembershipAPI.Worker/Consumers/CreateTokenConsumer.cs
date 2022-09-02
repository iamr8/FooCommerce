using FooCommerce.Domain.DbProvider;
using FooCommerce.MembershipAPI.Contracts.FaultedResponses;
using FooCommerce.MembershipAPI.Contracts.FaultedResponses.Enums;
using FooCommerce.MembershipAPI.Contracts.Requests;
using FooCommerce.MembershipAPI.Contracts.Responses;
using FooCommerce.MembershipAPI.Worker.Services;
using FooCommerce.MembershipAPI.Worker.Services.Repositories;

using MassTransit;

namespace FooCommerce.MembershipAPI.Worker.Consumers;

public class CreateTokenConsumer
    : IConsumer<CreateTokenRequest>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ITokenService _tokenService;

    public CreateTokenConsumer(IDbConnectionFactory dbConnectionFactory, ITokenService tokenService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _tokenService = tokenService;
    }

    public async Task Consume(ConsumeContext<CreateTokenRequest> context)
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();

        var communicationModel = await CommunicationService.GetModelByIdAsync(context.Message.UserCommunicationId, dbConnection, context.CancellationToken);
        if (communicationModel == null)
        {
            await context.RespondAsync<TokenCreationFaulted>(new
            {
                Fault = TokenCreationFault.CommunicationNotFound
            });
            return;
        }

        if (communicationModel.IsVerified)
        {
            await context.RespondAsync<TokenCreationFaulted>(new
            {
                Fault = TokenCreationFault.AlreadyEstablished
            });
            return;
        }

        var token = TokenService.GenerateToken(communicationModel.Type);
        var authToken = await _tokenService.CreateAuthTokenAsync(context.Message.UserCommunicationId, token, context.CancellationToken);
        if (authToken == null)
        {
            await context.RespondAsync<TokenCreationFaulted>(new
            {
                Fault = TokenCreationFault.TokenNotCreated
            });
            return;
        }

        var tokenUserModel = await CommunicationService.GetTokenUserModelAsync(communicationModel, dbConnection, context.CancellationToken);
        if (tokenUserModel == null)
        {
            await context.RespondAsync<TokenCreationFaulted>(new
            {
                Fault = TokenCreationFault.TokenUserNotCreated
            });
            return;
        }

        await context.RespondAsync<TokenCreationDone>(new
        {
            tokenUserModel.UserId,
            tokenUserModel.Name,
            tokenUserModel.Communication,
            authToken.Token
        });
    }
}