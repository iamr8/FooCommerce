using FooCommerce.Common.Protection;
using FooCommerce.DbProvider;
using FooCommerce.Domain.Enums;
using FooCommerce.IdentityAPI.Worker.Contracts;
using FooCommerce.IdentityAPI.Worker.Contracts.Enums;
using FooCommerce.IdentityAPI.Worker.Enums;
using FooCommerce.IdentityAPI.Worker.Services.Repositories;

using MassTransit;

namespace FooCommerce.IdentityAPI.Worker.Consumers;

public class GetUserClaimsConsumer
    : IConsumer<SignInRequest>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetUserClaimsConsumer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task Consume(ConsumeContext<SignInRequest> context)
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();

        var usernameType = context.Message.Username.Contains('@') ? CommType.Email : CommType.Sms;
        var userCredentialModel = await UserManager.GetUserModelAsync(usernameType, context.Message.Username, dbConnection);
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

        var communicationModel = await CommunicationsManager.GetModelByIdAsync(userCredentialModel.CommunicationId, dbConnection);
        if (!communicationModel.IsVerified)
        {
            await context.RespondAsync<UserClaimFindingFaulted>(new
            {
                Fault = UserClaimFindingFault.UserNotVerified,
                Communication = communicationModel
            });
            return;
        }

        var informationModels = await UserManager.GetUserInformationAsync(userCredentialModel.UserId, dbConnection);
        if (informationModels == null || !informationModels.Any())
        {
            await context.RespondAsync<UserClaimFindingFaulted>(new
            {
                Fault = UserClaimFindingFault.UserInformationMissing
            });
            return;
        }

        var settingModels = await UserManager.GetUserSettingsAsync(userCredentialModel.UserId, dbConnection);
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