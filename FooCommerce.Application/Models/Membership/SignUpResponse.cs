using FluentValidation.Results;

using FooCommerce.Domain.Enums;

namespace FooCommerce.Application.Models.Membership;

public struct SignUpResponse
{
    public List<ValidationFailure> Errors { get; init; }
    public JobStatus Status { get; init; }

    public static implicit operator SignUpResponse(JobStatus sts)
    {
        return new SignUpResponse { Status = sts };
    }

    public static explicit operator JobStatus(SignUpResponse resp)
    {
        return resp.Status;
    }
}