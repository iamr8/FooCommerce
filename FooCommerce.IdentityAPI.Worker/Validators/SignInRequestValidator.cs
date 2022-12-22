using FluentValidation;
using FooCommerce.IdentityAPI.Worker.Contracts;
using FooCommerce.IdentityAPI.Worker.Validators.PropertyValidators;

namespace FooCommerce.IdentityAPI.Worker.Validators;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(x => x.Username).SetValidator(new UsernameValidator());
        RuleFor(x => x.Password).SetValidator(new PasswordValidator());
    }
}