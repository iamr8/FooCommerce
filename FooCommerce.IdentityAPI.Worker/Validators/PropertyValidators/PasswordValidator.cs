using FluentValidation;

namespace FooCommerce.IdentityAPI.Worker.Validators.PropertyValidators;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(x => x).NotEmpty().WithMessage("Password is required.");
        RuleFor(x => x).MinimumLength(8).WithMessage("Password must be at least 8 characters.");
    }
}