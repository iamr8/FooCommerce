using System.Security.Claims;

using FooCommerce.Application.Models;

using FooCommerce.Domain.Enums;

namespace FooCommerce.MembershipAPI.Models;

public record SignInResponse : JobTaskResponse
{
    public SignInResponse()
    {
    }

    public SignInResponse(ClaimsPrincipal principal, params object[] properties)
    {
        ClaimsPrincipal = principal;
        Properties = properties;
    }
    public ClaimsPrincipal ClaimsPrincipal { get; }
    public IEnumerable<object> Properties { get; } = new List<object>(1);

    public static implicit operator SignInResponse(JobStatus sts)
    {
        return new SignInResponse { Status = sts };
    }

    public static explicit operator JobStatus(SignInResponse resp)
    {
        return resp.Status;
    }
}