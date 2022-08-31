using FooCommerce.Application.Models;
using FooCommerce.Domain.Enums;

namespace FooCommerce.MembershipAPI.Models;

public record RequestVerificationResponse : JobTaskResponse
{
    internal RequestVerificationResponse()
    {
    }
    public RequestVerificationResponse(Guid? communicationId, string token) : this()
    {
        CommunicationId = communicationId;
        Token = token;
    }

    public static implicit operator RequestVerificationResponse(JobStatus sts)
    {
        return new RequestVerificationResponse { Status = sts };
    }

    public static explicit operator JobStatus(RequestVerificationResponse resp)
    {
        return resp.Status;
    }

    public Guid? CommunicationId { get; }
    public string Token { get; }
}