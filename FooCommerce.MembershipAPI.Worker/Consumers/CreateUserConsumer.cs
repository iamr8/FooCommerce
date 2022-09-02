using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.Enums;
using FooCommerce.MembershipAPI.Contracts.FaultedResponses;
using FooCommerce.MembershipAPI.Contracts.FaultedResponses.Enums;
using FooCommerce.MembershipAPI.Contracts.Requests;
using FooCommerce.MembershipAPI.Contracts.Responses;
using FooCommerce.MembershipAPI.Enums;
using FooCommerce.MembershipAPI.Worker.Services;
using FooCommerce.MembershipAPI.Worker.Services.Repositories;

using MassTransit;

namespace FooCommerce.MembershipAPI.Worker.Consumers;

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