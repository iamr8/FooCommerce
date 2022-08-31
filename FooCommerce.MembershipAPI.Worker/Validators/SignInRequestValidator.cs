using FluentValidation;

using FooCommerce.MembershipAPI.Models;
using FooCommerce.MembershipAPI.Validators;

namespace FooCommerce.MembershipAPI.Worker.Validators;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(x => x.Username).SetValidator(new UsernameValidator());
        RuleFor(x => x.Password).SetValidator(new PasswordValidator());
    }
}