using FooCommerce.Domain.Enums;
using FooCommerce.MembershipService.Contracts;
using FooCommerce.MembershipService.Enums;
using FooCommerce.MembershipService.Services;
using MassTransit;

namespace FooCommerce.MembershipService.Consumers;

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
            await context.RespondAsync<UserCreated>(new
            {
                Success = false,
                Message = "User already exists",
            });
            return;
        }

        var role = await _user.GetRoleAsync(RoleType.NormalUser, context.CancellationToken);

        try
        {
            var createdUserModel = await _user.CreateUserAsync(context.Message, role);
            await context.RespondAsync<UserCreated>(new
            {
                CommId = createdUserModel.Communication.Id,
                Success = true
            });
        }
        catch (Exception e)
        {
            await context.RespondAsync<UserCreated>(new
            {
                Success = false,
                Message = e.Message
            });
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