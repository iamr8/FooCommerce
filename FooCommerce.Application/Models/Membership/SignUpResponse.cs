using FluentValidation.Results;

using FooCommerce.Domain.Enums;

namespace FooCommerce.Application.Models.Membership;

public record SignUpResponse
{
    public List<ValidationFailure> Errors { get; init; }
    public Status Status { get; init; }

    public static implicit operator SignUpResponse(Status sts)
    {
        return new SignUpResponse { Status = sts };
    }

    public static explicit operator Status(SignUpResponse resp)
    {
        return resp.Status;
    }
}