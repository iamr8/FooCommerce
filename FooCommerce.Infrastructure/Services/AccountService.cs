using FooCommerce.Application.Communications.Enums;
using FooCommerce.Domain.Enums;
using FooCommerce.MembershipAPI.Models;
using FooCommerce.MembershipAPI.Services;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Models;

using MassTransit;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace FooCommerce.Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly IUserService _userService;
    private readonly IVerificationService _verificationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBus _bus;

    public AccountService(
        IUserService userService,
        IVerificationService verificationService,
        IHttpContextAccessor httpContextAccessor,
        IBus bus)
    {
        _bus = bus;
        _verificationService = verificationService;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<JobStatus> SignInAsync(SignInRequest model, string returnUrl = null, CancellationToken cancellationToken = default)
    {
        var output = await _userService.SignInAsync(model, returnUrl, cancellationToken);
        if (output.Status != JobStatus.Success)
            return output.Status;

        var authenticationProps = output.Properties.OfType<AuthenticationProperties>().Single();
        await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, output.ClaimsPrincipal, authenticationProps);
        return JobStatus.Success;
    }

    public async Task<JobStatus> SignUpAsync(SignUpRequest model, CancellationToken cancellationToken = default)
    {
        var output = await _userService.SignUpAsync(model, cancellationToken);
        if (output.Status != JobStatus.Success)
            return output.Status;

        var verif = await RequestVerificationAsync(output.CommunicationType, output.CommunicationAddress, cancellationToken);
        if (verif != JobStatus.Success)
            return verif;

        // Sending notification has been queued
        // We lead user into Confirm Page, in order to keep them waiting for the incoming token
        return JobStatus.NeedVerifyEmail;
    }

    private async Task<JobStatus> RequestVerificationAsync(CommunicationType type, string value, CancellationToken cancellationToken = default)
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