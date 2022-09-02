using FluentValidation.Results;

namespace FooCommerce.Infrastructure.Models;

public interface IJobTaskResponse
{
    IEnumerable<ValidationFailure> Errors { get; init; }
    JobStatus Status { get; init; }
}