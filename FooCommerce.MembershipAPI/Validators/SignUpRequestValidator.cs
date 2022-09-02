using FluentValidation;
using FooCommerce.MembershipAPI.Contracts.Requests;
using FooCommerce.MembershipAPI.Validators.PropertyValidators;

namespace FooCommerce.MembershipAPI.Validators;

public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.Email).SetValidator(new EmailValidator());
        RuleFor(x => x.Password).SetValidator(new PasswordValidator());

        //if (locationService is not null)
        //    RuleFor(x => x.Country).SetValidator(new CountryValidator(locationService));
    }
}