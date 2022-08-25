using FluentValidation;

namespace FooCommerce.Application.Membership.Validators;

public class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(x => x).NotEmpty().WithMessage("Email is required.");
        RuleFor(x => x).EmailAddress().WithMessage("Email is not valid.");
    }
}