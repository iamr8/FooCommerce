using FooCommerce.Application.Communications.Enums;
using FooCommerce.Application.DbProvider;
using FooCommerce.Domain.Enums;
using FooCommerce.MembershipAPI.Models;
using FooCommerce.MembershipAPI.Services;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Models;

using MassTransit;

namespace FooCommerce.Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly IBus _bus;
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IVerificationService _verificationService;
    private readonly IUserService _userService;

    public AccountService(IDbConnectionFactory dbConnectionFactory, IBus bus, IVerificationService verificationService, IUserService userService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _bus = bus;
        _verificationService = verificationService;
        _userService = userService;
    }

    public async Task<SignInResponse> SignInAsync(SignInRequest model, string returnUrl = null, CancellationToken cancellationToken = default)
    {
        var output = await _userService.SignInAsync(model, returnUrl, cancellationToken);
        // await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProps);
        return output;
    }

    public async Task<JobStatus> RequestVerificationAsync(CommunicationType type, string value, CancellationToken cancellationToken = default)
    {
        var result = await _verificationService.RequestVerificationAsync(type, value, cancellationToken);
        if (result != JobStatus.Success)
            return result.Status;

        await _bus.Publish<QueueNotification>(new
        {
            Action = type switch
            {
                CommunicationType.Email_Message => NotificationAction.Verification_Request_Email,
                CommunicationType.Mobile_Sms => NotificationAction.Verification_Request_Mobile,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            },
            Receiver = new NotificationReceiverProvider(NotificationReceiverStrategy.ByUserCommunicationId, result.CommunicationId.Value),
            Content = (IEnumerable<INotificationContent>)Enumerable.Range(0, 1).Select(_ => new NotificationFormatting("authToken", result.Token))
        }, cancellationToken);
        return JobStatus.Success;
    }
}