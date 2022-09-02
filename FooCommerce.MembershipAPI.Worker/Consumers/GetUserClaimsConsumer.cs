using FooCommerce.Common.Protection;
using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.Enums;
using FooCommerce.MembershipAPI.Contracts.FaultedResponses;
using FooCommerce.MembershipAPI.Contracts.FaultedResponses.Enums;
using FooCommerce.MembershipAPI.Contracts.Requests;
using FooCommerce.MembershipAPI.Contracts.Responses;
using FooCommerce.MembershipAPI.Worker.Services.Repositories;

using MassTransit;

using Microsoft.AspNetCore.Authentication;

namespace FooCommerce.MembershipAPI.Worker.Consumers;

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

        var usernameType = context.Message.Username.Contains('@') ? CommunicationType.Email_Message : CommunicationType.Mobile_Sms;
        var userCredentialModel = await UserService.GetUserModelAsync(usernameType, context.Message.Username, dbConnection, context.CancellationToken);
        if (userCredentialModel == null)
        {
            await context.RespondAsync<UserClaimFindingFaulted>(new
            {
                Fault = UserClaimFindingFault.UserNotFound
            });
            return;
        }

        var (passwordMatched, passwordNeedsUpgrade) = DataProtector.Check(userCredentialModel.Hash, context.Message.Password, 32, 10_000);
        if (!passwordMatched)
        {
            await context.RespondAsync<UserClaimFindingFaulted>(new
            {
                Fault = UserClaimFindingFault.UserIncorrectPassword
            });
            return;
        }

        var communicationModel = await CommunicationService.GetModelByIdAsync(userCredentialModel.CommunicationId, dbConnection, context.CancellationToken);
        if (!communicationModel.IsVerified)
        {
            await context.RespondAsync<UserClaimFindingFaulted>(new
            {
                Fault = UserClaimFindingFault.UserNotVerified,
                Communication = communicationModel
            });
            return;
        }

        var informationModels = await UserService.GetUserInformationAsync(userCredentialModel.UserId, dbConnection, context.CancellationToken);
        if (informationModels == null || !informationModels.Any())
        {
            await context.RespondAsync<UserClaimFindingFaulted>(new
            {
                Fault = UserClaimFindingFault.UserInformationMissing
            });
            return;
        }

        var settingModels = await UserService.GetUserSettingsAsync(userCredentialModel.UserId, dbConnection, context.CancellationToken);
        if (settingModels == null || !settingModels.Any())
        {
            await context.RespondAsync<UserClaimFindingFaulted>(new
            {
                Fault = UserClaimFindingFault.UserSettingsMissing
            });
            return;
        }

        var claimsPrincipal = UserService.GetClaimsPrincipal(context.Message.Username, userCredentialModel, communicationModel, informationModels, settingModels);
        var authenticationProps = new AuthenticationProperties
        {
            AllowRefresh = true,
            IsPersistent = context.Message.Remember,
            RedirectUri = context.Message.ReturnUrl,
            IssuedUtc = DateTimeOffset.UtcNow,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
        };

        await context.RespondAsync<UserClaimFound>(new
        {
            ClaimsPrincipal = claimsPrincipal,
            Properties = authenticationProps
        });
    }
}