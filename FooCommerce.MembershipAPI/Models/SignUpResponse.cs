using FooCommerce.Application.Communications.Enums;
using FooCommerce.Application.Models;

using FooCommerce.Domain.Enums;

namespace FooCommerce.MembershipAPI.Models;

public record SignUpResponse : JobTaskResponse
{
    public CommunicationType CommunicationType { get; set; }
    public string CommunicationAddress { get; set; }
    public static implicit operator SignUpResponse(JobStatus sts)
    {
        return new SignUpResponse { Status = sts };
    }

    public static explicit operator JobStatus(SignUpResponse resp)
    {
        return resp.Status;
    }
}