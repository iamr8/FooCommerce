using FluentValidation;
using FooCommerce.IdentityAPI.Contracts.Requests;
using FooCommerce.IdentityAPI.Validators.PropertyValidators;

namespace FooCommerce.IdentityAPI.Validators;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(x => x.Username).SetValidator(new UsernameValidator());
        RuleFor(x => x.Password).SetValidator(new PasswordValidator());
    }
}