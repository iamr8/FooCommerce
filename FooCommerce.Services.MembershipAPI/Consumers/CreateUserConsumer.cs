using FooCommerce.Domain.Enums;
using FooCommerce.Services.MembershipAPI.Contracts;
using FooCommerce.Services.MembershipAPI.Enums;
using FooCommerce.Services.MembershipAPI.Services;

using MassTransit;

namespace FooCommerce.Services.MembershipAPI.Consumers;

public class CreateUserConsumer :
    IConsumer<CreateUser>,
    IConsumer<Fault<CreateUser>>
{
    private readonly ICommunicationsManager _communicationsManager;
    private readonly IUserManager _user;

    private readonly ILogger<CreateUserConsumer> _logger;

    public CreateUserConsumer(
        IUserManager user,
        ICommunicationsManager communicationsManager, ILogger<CreateUserConsumer> logger)
    {
        _user = user;
        _communicationsManager = communicationsManager;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CreateUser> context)
    {
        var establishedUserCommunicationId = await _communicationsManager.GetVerifiedCommunicationIdAsync(CommType.Email, context.Message.Email);
        if (establishedUserCommunicationId.HasValue)
        {
            await context.RespondAsync<UserCreationStatus>(new
            {
                Fault = UserCreationFault.EmailAlreadyEstablished
            });
            return;
        }

        var role = await _user.GetRoleAsync(RoleType.NormalUser, context.CancellationToken);

        try
        {
            var createdUserModel = await _user.CreateUserAsync(context.Message, role);
            await context.RespondAsync<UserCreationStatus>(new
            {
                CommunicationId = createdUserModel.Communication.Id,
            });
        }
        catch (Exception e)
        {
            await context.RespondAsync<UserCreationStatus>(new
            {
                ExceptionMessage = e.Message
            });
            return;
        }
    }

    public Task Consume(ConsumeContext<Fault<CreateUser>> context)
    {
        for (int i = 0; i < context.Message.Exceptions.Length; i++)
        {
            _logger.LogError(context.Message.Exceptions[i].Message);
        }

        return Task.CompletedTask;
    }
}