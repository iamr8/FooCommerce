namespace FooCommerce.Infrastructure.Services.Repositories;

public class AccountService : IAccountService
{
    //private readonly ILocationService _locationService;
    //private readonly IHttpContextAccessor _httpContextAccessor;
    //private readonly INotificationBusService _notificationBusService;
    //private readonly IBus _bus;

    //public AccountService(
    //    IHttpContextAccessor httpContextAccessor,
    //    IBus bus, ILocationService locationService, INotificationBusService notificationBusService)
    //{
    //    _httpContextAccessor = httpContextAccessor;
    //    _bus = bus;
    //    _locationService = locationService;
    //    _notificationBusService = notificationBusService;
    //}

    //public async Task<JobStatus> SignInAsync(SignInRequest model, CancellationToken cancellationToken = default)
    //{
    //    var validator = new SignInRequestValidator();
    //    var validationResult = await validator.ValidateAsync(model, cancellationToken);
    //    if (!validationResult.IsValid)
    //        return JobStatus.InputDataNotValid;

    //    var requestClient = _bus.CreateRequestClient<SignInRequest>(new Uri("exchange:get-user-claims"));
    //    var (found, faulted) = await requestClient.GetResponse<UserClaimFound, UserClaimFindingFaulted>(new
    //    {
    //        model.Username,
    //        model.Password,
    //        model.Remember,
    //        model.ReturnUrl
    //    }, cancellationToken);

    //    if (found.IsCompletedSuccessfully)
    //    {
    //        var response = await found;

    //        var authenticationProps = new AuthenticationProperties
    //        {
    //            AllowRefresh = true,
    //            IsPersistent = model.Remember,
    //            RedirectUri = model.ReturnUrl,
    //            IssuedUtc = DateTimeOffset.UtcNow,
    //            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
    //        };
    //        await _httpContextAccessor.HttpContext!.SignInAsync(response.Message.ClaimsPrincipal, authenticationProps);
    //        return JobStatus.Success;
    //    }
    //    else
    //    {
    //        var response = await faulted;
    //        switch (response.Message.Fault)
    //        {
    //            case UserClaimFindingFault.UserNotFound:
    //            case UserClaimFindingFault.UserIncorrectPassword:
    //                return JobStatus.IncorrectUsernameOrPassword;

    //            case UserClaimFindingFault.UserNotVerified:
    //                {
    //                    // var result = response.Message.Communication;
    //                    // var requestVerification = await RequestVerificationAsync(result.Id, cancellationToken);
    //                    // TODO: perform some works...
    //                    // redirect to Verification page
    //                    return JobStatus.Success;
    //                }

    //            case UserClaimFindingFault.UserInformationMissing:
    //            case UserClaimFindingFault.UserSettingsMissing:
    //            default:
    //                return JobStatus.Failed;
    //        }
    //    }
    //}

    //public async Task<JobStatus> SignUpAsync(SignUpRequest model, CancellationToken cancellationToken = default)
    //{
    //    // TODO: must check Country
    //    var validator = new SignUpRequestValidator();
    //    var validationResult = await validator.ValidateAsync(model, cancellationToken);
    //    if (!validationResult.IsValid)
    //        return JobStatus.InputDataNotValid;

    //    var validCountry = await _locationService.IsCountryValidAsync(model.Country, cancellationToken);
    //    if (!validCountry)
    //        return JobStatus.InputDataNotValid;

    //    var requestClient = _bus.CreateRequestClient<SignUpRequest>(new Uri("exchange:create-user"));
    //    var (created, failed) = await requestClient.GetResponse<UserCreated, UserCreationFaulted>(new
    //    {
    //        model.Email,
    //        model.Password,
    //        model.Country,
    //        model.FirstName,
    //        model.LastName
    //    }, cancellationToken);

    //    if (created.IsCompletedSuccessfully)
    //    {
    //        var response = await created;
    //        // TODO: perform some works...
    //        // redirect to Verification page
    //        return JobStatus.NeedVerifyEmail;
    //    }
    //    else
    //    {
    //        var response = await failed;
    //        switch (response.Message.Fault)
    //        {
    //            case UserCreationFault.EmailAlreadyEstablished:
    //            default:
    //                return JobStatus.CommunicationAlreadyEstablished;
    //        }
    //    }
    //}

    //public async Task<JobStatus> FulfillVerificationAsync(string token, TokenRequestPurpose purpose, CancellationToken cancellationToken = default)
    //{
    //    //var requestClient = _bus.CreateRequestClient<ValidateCode>(new Uri("exchange:authorize-token"));
    //    //var (authorized, failed) = await requestClient.GetResponse<TokenValidated, TokenValidationFailed>(new
    //    //{
    //    //    Token = token,
    //    //    Purpose = purpose
    //    //}, cancellationToken);

    //    //if (authorized.IsCompletedSuccessfully)
    //    //{
    //    //    var response = await authorized;
    //    //    // TODO: perform some works
    //    //    return JobStatus.Success;
    //    //}
    //    //else
    //    //{
    //    //    var response = await failed;
    //    //    return JobStatus.Failed;
    //    //}
    //    return JobStatus.Failed;
    //}

    //public async Task<JobStatus> RequestVerificationAsync(string auth, CancellationToken cancellationToken = default)
    //{
    //    //var requestClient = _bus.CreateRequestClient<GenerateCode>(new Uri("exchange:create-token"));
    //    //var (created, failed) = await requestClient.GetResponse<TokenGenerated, TokenGenerationFaulted>(new
    //    //{
    //    //    UserCommunicationId = Guid.Empty
    //    //}, cancellationToken);

    //    //if (created.IsCompletedSuccessfully)
    //    //{
    //    //    var response = await created;
    //    //    //var tokenUserModel = await CommunicationsManager.GetTokenUserModelAsync(communicationModel, dbConnection);
    //    //    //if (tokenUserModel == null)
    //    //    //{
    //    //    //    await context.RespondAsync<TokenGenerationFailed>(new
    //    //    //    {
    //    //    //        Fault = TokenCreationFault.TokenUserNotFetched
    //    //    //    });

    //    //    //    return;
    //    //    //}
    //    //    //var purpose = response.Message.Communication.Type switch
    //    //    //{
    //    //    //    CommunicationType.Email => NotificationPurpose.Verification_Request_Email,
    //    //    //    CommunicationType.Sms => NotificationPurpose.Verification_Request_Mobile,
    //    //    //    _ => throw new ArgumentOutOfRangeException(nameof(response.Message.Communication.Type), response.Message.Communication.Type, null)
    //    //    //};

    //    //    //var receiverProvider = new NotificationReceiverProvider
    //    //    //{
    //    //    //    UserId = response.Message.UserId,
    //    //    //    Name = response.Message.Name,
    //    //    //    Communications = new Dictionary<CommunicationType, string> {{response.Message.Communication.Type, response.Message.Communication.Value}}
    //    //    //};
    //    //    //ContextRequestInfo
    //    //    //var content = Enumerable.Range(0, 1).Select(_ => new NotificationFormatter("authToken", response.Message.Token));

    //    //    //await _notificationBusService.EnqueueAsync(
    //    //    //    options =>
    //    //    //    {
    //    //    //        options.Purpose = response.Message.Communication.Type switch
    //    //    //        {
    //    //    //            CommunicationType.Email => NotificationPurpose.Verification_Request_Email,
    //    //    //            CommunicationType.Sms => NotificationPurpose.Verification_Request_Mobile,
    //    //    //            _ => throw new ArgumentOutOfRangeException(nameof(response.Message.Communication.Type), response.Message.Communication.Type, null)
    //    //    //        }
    //    //    //    },
    //    //    //    _httpContextAccessor.HttpContext.GetRequestInfo(),
    //    //    //    cancellationToken);
    //    //    return JobStatus.Success;
    //    //}
    //    //else
    //    //{
    //    //    var response = await failed;
    //    //    switch (response.Message.Fault)
    //    //    {
    //    //        case TokenGenerationFault.CommunicationNeedToBeNotVerified:
    //    //            return JobStatus.CommunicationAlreadyEstablished;

    //    //        case TokenGenerationFault.TokenNotCreated:
    //    //        case TokenGenerationFault.CommunicationNotFound:
    //    //        default:
    //    //            return JobStatus.Failed;
    //    //    }
    //    //}
    //    return JobStatus.Failed;
    //}
}