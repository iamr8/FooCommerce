using FluentValidation;
using FooCommerce.MembershipAPI.Contracts.Requests;
using FooCommerce.MembershipAPI.Validators.PropertyValidators;

namespace FooCommerce.MembershipAPI.Validators;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(x => x.Username).SetValidator(new UsernameValidator());
        RuleFor(x => x.Password).SetValidator(new PasswordValidator());
    }
}