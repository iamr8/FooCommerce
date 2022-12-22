using FluentValidation;

using FooCommerce.Infrastructure.Membership.Contracts;
using FooCommerce.Infrastructure.Membership.Validators.PropertyValidators;

namespace FooCommerce.Infrastructure.Membership.Validators;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(x => x.Username).SetValidator(new UsernameValidator());
        RuleFor(x => x.Password).SetValidator(new PasswordValidator());
    }
}