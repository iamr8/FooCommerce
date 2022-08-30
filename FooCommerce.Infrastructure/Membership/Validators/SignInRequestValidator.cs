using FluentValidation;

using FooCommerce.Application.Membership.Models;
using FooCommerce.Infrastructure.FluentValidators;

namespace FooCommerce.Infrastructure.Membership.Validators;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(x => x.Username).SetValidator(new UsernameValidator());
        RuleFor(x => x.Password).SetValidator(new PasswordValidator());
    }
}