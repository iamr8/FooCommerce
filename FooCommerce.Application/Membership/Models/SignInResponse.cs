using System.Security.Claims;

using FooCommerce.Application.Models;

using FooCommerce.Domain.Enums;

using Microsoft.AspNetCore.Authentication;

namespace FooCommerce.Application.Membership.Models;

public record SignInResponse : JobTaskResponse
{
    private SignInResponse(ClaimsPrincipal claimsPrincipal, AuthenticationProperties authenticationProperties)
    {
        ClaimsPrincipal = claimsPrincipal;
        AuthenticationProperties = authenticationProperties;
        Status = JobStatus.Success;
    }

    public static SignInResponse CreateInstance(ClaimsPrincipal claimsPrincipal, AuthenticationProperties authenticationProperties)
    {
        return new SignInResponse(claimsPrincipal, authenticationProperties);
    }

    public SignInResponse()
    {
    }

    public ClaimsPrincipal ClaimsPrincipal { get; } = null!;
    public AuthenticationProperties AuthenticationProperties { get; } = null!;

    public static implicit operator SignInResponse(JobStatus sts)
    {
        return new SignInResponse { Status = sts };
    }

    public static explicit operator JobStatus(SignInResponse resp)
    {
        return resp.Status;
    }
}