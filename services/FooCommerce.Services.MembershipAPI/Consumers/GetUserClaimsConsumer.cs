using FooCommerce.Common.Protection;
using FooCommerce.Domain.Enums;
using FooCommerce.MembershipService.Contracts;
using FooCommerce.MembershipService.Enums;
using FooCommerce.MembershipService.Services;
using FooCommerce.MembershipService.Services.Repositories;
using MassTransit;

namespace FooCommerce.MembershipService.Consumers;

public class GetUserClaimsConsumer
    : IConsumer<SignInRequest>
{
    private readonly IUserManager _userManager;
    private readonly ICommunicationsManager _communicationsManager;

    public GetUserClaimsConsumer(IUserManager userManager, ICommunicationsManager communicationsManager)
    {
        _userManager = userManager;
        _communicationsManager = communicationsManager;
    }

    public async Task Consume(ConsumeContext<SignInRequest> context)
    {
        var usernameType = context.Message.Username.Contains('@') ? CommType.Email : CommType.Sms;
        var userCredentialModel = await _userManager.GetUserModelAsync(usernameType, context.Message.Username);
        if (userCredentialModel == null)
        {
            await context.RespondAsync<UserClaimFindingFaulted>(new
            {
                Fault = UserClaimFindingFault.UserNotFound
            });
            return;
        }

        var (passwordMatched, _) = DataProtector.Check(userCredentialModel.Hash, context.Message.Password, 32, 10_000);
        if (!passwordMatched)
        {
            await context.RespondAsync<UserClaimFindingFaulted>(new
            {
                Fault = UserClaimFindingFault.UserIncorrectPassword
            });
            return;
        }

        var communicationModel = await _communicationsManager.GetModelByIdAsync(userCredentialModel.CommunicationId);
        if (!communicationModel.IsVerified)
        {
            await context.RespondAsync<UserClaimFindingFaulted>(new
            {
                Fault = UserClaimFindingFault.UserNotVerified,
                Communication = communicationModel
            });
            return;
        }

        var informationModels = await _userManager.GetUserInformationAsync(userCredentialModel.UserId);
        if (informationModels == null || !informationModels.Any())
        {
            await context.RespondAsync<UserClaimFindingFaulted>(new
            {
                Fault = UserClaimFindingFault.UserInformationMissing
            });
            return;
        }

        var settingModels = await _userManager.GetUserSettingsAsync(userCredentialModel.UserId);
        if (settingModels == null || !settingModels.Any())
        {
            await context.RespondAsync<UserClaimFindingFaulted>(new
            {
                Fault = UserClaimFindingFault.UserSettingsMissing
            });
            return;
        }

        var claimsPrincipal = UserManager.GetClaimsPrincipal(context.Message.Username, userCredentialModel, communicationModel, informationModels, settingModels);

        await context.RespondAsync<UserClaimFound>(new
        {
            ClaimsPrincipal = claimsPrincipal,
        });
    }
}