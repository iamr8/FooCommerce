using FooCommerce.DbProvider;
using FooCommerce.Domain.Enums;
using FooCommerce.IdentityAPI.Contracts.FaultedResponses;
using FooCommerce.IdentityAPI.Contracts.FaultedResponses.Enums;
using FooCommerce.IdentityAPI.Contracts.Requests;
using FooCommerce.IdentityAPI.Contracts.Responses;
using FooCommerce.IdentityAPI.Enums;
using FooCommerce.IdentityAPI.Worker.Services;
using FooCommerce.IdentityAPI.Worker.Services.Repositories;

using MassTransit;

namespace FooCommerce.IdentityAPI.Worker.Consumers;

public class CreateUserConsumer
    : IConsumer<SignUpRequest>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IUserService _userService;

    public CreateUserConsumer(IDbConnectionFactory dbConnectionFactory, IUserService userService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _userService = userService;
    }

    public async Task Consume(ConsumeContext<SignUpRequest> context)
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();

        var establishedUserCommunicationId = await CommunicationService.GetVerifiedCommunicationIdAsync(CommunicationType.Email_Message, context.Message.Email, dbConnection, context.CancellationToken);
        if (establishedUserCommunicationId.HasValue)
        {
            await context.RespondAsync<UserCreationFaulted>(new
            {
                Fault = UserCreationFault.EmailAlreadyEstablished
            });
            return;
        }

        var role = await _userService.GetRoleAsync(RoleType.NormalUser, dbConnection, context.CancellationToken);
        var createdUserModel = await _userService.CreateUserAsync(context.Message, role, context.CancellationToken);
        if (createdUserModel == null)
        {
            await context.RespondAsync<UserCreationFaulted>(new
            {
                Fault = UserCreationFault.DatabaseTransactionFailed
            });
            return;
        }

        await context.RespondAsync<UserCreationDone>(new
        {
            CommunicationId = createdUserModel.Communication.Id,
        });
    }
}