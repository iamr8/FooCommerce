using FluentValidation.Results;

using FooCommerce.Domain.Enums;

namespace FooCommerce.Application.Models;

public record JobTaskResponse
{
    public JobTaskResponse()
    {
        Status = JobStatus.Success;
    }

    public IEnumerable<ValidationFailure> Errors { get; init; }
    public JobStatus Status { get; init; }

    public static implicit operator JobTaskResponse(JobStatus sts)
    {
        return new JobTaskResponse { Status = sts };
    }

    public static explicit operator JobStatus(JobTaskResponse resp)
    {
        return resp.Status;
    }
}