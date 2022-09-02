using FluentValidation.Results;

namespace FooCommerce.Infrastructure.Models;

public abstract record JobTaskResponse : IJobTaskResponse
{
    protected JobTaskResponse(JobStatus status)
    {
        Status = status;
    }

    protected JobTaskResponse(JobStatus status, IEnumerable<ValidationFailure> errors) : this(status)
    {
        Errors = errors;
    }
    protected JobTaskResponse() : this(JobStatus.Success)
    {
    }

    public virtual IEnumerable<ValidationFailure> Errors { get; init; }
    public virtual JobStatus Status { get; init; }
}