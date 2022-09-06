using FooCommerce.AspNetCoreExtensions.Helpers;
using FooCommerce.Domain.ContextRequest;
using FooCommerce.Domain.Enums;
using FooCommerce.IdentityAPI.Contracts.FaultedResponses;
using FooCommerce.IdentityAPI.Contracts.FaultedResponses.Enums;
using FooCommerce.IdentityAPI.Contracts.Requests;
using FooCommerce.IdentityAPI.Contracts.Responses;
using FooCommerce.IdentityAPI.Validators;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Models;

using MassTransit;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace FooCommerce.Infrastructure.Services.Repositories;

public class AccountService : IAccountService
{
    private readonly ILocationService _locationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBus _bus;

    public AccountService(
        IHttpContextAccessor httpContextAccessor,
        IBus bus, ILocationService locationService)
    {
        _httpContextAccessor = httpContextAccessor;
        _bus = bus;
        _locationService = locationService;
    }

    public async Task<JobStatus> SignInAsync(SignInRequest model, CancellationToken cancellationToken = default)
    {
        var validator = new SignInRequestValidator();
        var validationResult = await validator.ValidateAsync(model, cancellationToken);
        if (!validationResult.IsValid)
            return JobStatus.InputDataNotValid;

        var requestClient = _bus.CreateRequestClient<SignInRequest>(new Uri("exchange:get-user-claims"));
        var (prepared, faulted) = await requestClient.GetResponse<UserClaimFound, UserClaimFindingFaulted>(new
        {
            model.Username,
            model.Password,
            model.Remember,
            model.ReturnUrl
        }, cancellationToken);

        if (faulted.IsCompleted)
        {
            var response = await faulted;
            switch (response.Message.Fault)
            {
                case UserClaimFindingFault.UserNotFound:
                case UserClaimFindingFault.UserIncorrectPassword:
                    return JobStatus.IncorrectUsernameOrPassword;

                case UserClaimFindingFault.UserNotVerified:
                    {
                        // var result = response.Message.Communication;
                        // var requestVerification = await RequestVerificationAsync(result.Id, cancellationToken);
                        // TODO: perform some works...
                        // redirect to Verification page
                        return JobStatus.Success;
                    }

                case UserClaimFindingFault.UserInformationMissing:
                case UserClaimFindingFault.UserSettingsMissing:
                default:
                    return JobStatus.Failed;
            }
        }
        else
        {
            var response = await prepared;

            var authenticationProps = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = model.Remember,
                RedirectUri = model.ReturnUrl,
                IssuedUtc = DateTimeOffset.UtcNow,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
            };
            await _httpContextAccessor.HttpContext!.SignInAsync(response.Message.ClaimsPrincipal, authenticationProps);
            return JobStatus.Success;
        }
    }

    public async Task<JobStatus> SignUpAsync(SignUpRequest model, CancellationToken cancellationToken = default)
    {
        // TODO: must check Country
        var validator = new SignUpRequestValidator();
        var validationResult = await validator.ValidateAsync(model, cancellationToken);
        if (!validationResult.IsValid)
            return JobStatus.InputDataNotValid;

        var validCountry = await _locationService.IsCountryValidAsync(model.Country, cancellationToken);
        if (!validCountry)
            return JobStatus.InputDataNotValid;

        var requestClient = _bus.CreateRequestClient<SignUpRequest>(new Uri("exchange:create-user"));
        var (done, faulted) = await requestClient.GetResponse<UserCreationDone, UserCreationFaulted>(new
        {
            model.Email,
            model.Password,
            model.Country,
            model.FirstName,
            model.LastName
        }, cancellationToken);

        if (faulted.IsCompleted)
        {
            var response = await faulted;
            switch (response.Message.Fault)
            {
                case UserCreationFault.EmailAlreadyEstablished:
                default:
                    return JobStatus.CommunicationAlreadyEstablished;
            }
        }
        else
        {
            var response = await done;
            // TODO: perform some works...
            // redirect to Verification page
            return JobStatus.NeedVerifyEmail;
        }
    }

    //public async Task<JobStatus> FulfillVerificationAsync(string value, CancellationToken cancellationToken = default)
    //{
    //    await _verificationService.FulfillVerificationAsync()
    //    // perform some works

    //    return JobStatus.Success;
    //}

    public async Task<JobStatus> RequestVerificationAsync(string auth, CancellationToken cancellationToken = default)
    {
        var requestClient = _bus.CreateRequestClient<CreateTokenRequest>(new Uri("exchange:create-token"));
        var (done, faulted) = await requestClient.GetResponse<TokenCreationDone, TokenCreationFaulted>(new
        {
            UserCommunicationId = Guid.Empty
        }, cancellationToken);

        if (faulted.IsCompleted)
        {
            var response = await faulted;
            switch (response.Message.Fault)
            {
                case TokenCreationFault.AlreadyEstablished:
                    return JobStatus.CommunicationAlreadyEstablished;

                case TokenCreationFault.TokenNotCreated:
                case TokenCreationFault.CommunicationNotFound:
                default:
                    return JobStatus.Failed;
            }
        }
        else
        {
            var response = await done;

            await _bus.Publish<QueueNotification>(new
            {
                Action = response.Message.Communication.Type switch
                {
                    CommunicationType.Email_Message => NotificationAction.Verification_Request_Email,
                    CommunicationType.Mobile_Sms => NotificationAction.Verification_Request_Mobile,
                    _ => throw new ArgumentOutOfRangeException(nameof(response.Message.Communication.Type), response.Message.Communication.Type, null)
                },
                ReceiverProvider = new NotificationReceiverProvider
                {
                    UserId = response.Message.UserId,
                    Name = response.Message.Name,
                    Communications = new Dictionary<CommunicationType, string> { { response.Message.Communication.Type, response.Message.Communication.Value } }
                },
                Content = Enumerable.Range(0, 1).Select(_ => new NotificationFormatter("authToken", response.Message.Token)),
                RequestInfo = (ContextRequestInfo)_httpContextAccessor.HttpContext.GetRequestInfo()
            }, cancellationToken);
            return JobStatus.Success;
        }
    }
}