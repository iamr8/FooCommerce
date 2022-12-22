using FooCommerce.Domain.Enums;
using FooCommerce.IdentityAPI.Worker.Contracts;
using FooCommerce.IdentityAPI.Worker.Contracts.Enums;
using FooCommerce.IdentityAPI.Worker.Enums;
using FooCommerce.IdentityAPI.Worker.Services;

using MassTransit;

namespace FooCommerce.IdentityAPI.Worker.Consumers;

public class CreateUserConsumer
    : IConsumer<SignUpRequest>
{
    private readonly ICommunicationsManagerService _communicationsManagerService;
    private readonly IUserManagerService _userService;

    public CreateUserConsumer(
        IUserManagerService userService,
        ICommunicationsManagerService communicationsManagerService
        )
    {
        _userService = userService;
        _communicationsManagerService = communicationsManagerService;
    }

    public async Task Consume(ConsumeContext<SignUpRequest> context)
    {
        var establishedUserCommunicationId = await _communicationsManagerService.GetVerifiedCommunicationIdAsync(CommType.Email, context.Message.Email);
        if (establishedUserCommunicationId.HasValue)
        {
            await context.RespondAsync<UserCreationFaulted>(new
            {
                Fault = UserCreationFault.EmailAlreadyEstablished
            });
            return;
        }

        var role = await _userService.GetRoleAsync(RoleType.NormalUser, context.CancellationToken);
        var createdUserModel = await _userService.CreateUserAsync(context.Message, role, context.CancellationToken);
        if (createdUserModel == null)
        {
            await context.RespondAsync<UserCreationFaulted>(new
            {
                Fault = UserCreationFault.DatabaseTransactionFailed
            });
            return;
        }

        await context.RespondAsync<UserCreated>(new
        {
            CommunicationId = createdUserModel.Communication.Id,
        });
    }
}