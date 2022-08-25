using FluentValidation;

using FooCommerce.Application.Listings.Services;
using FooCommerce.Application.Membership.Models;
using FooCommerce.Application.Membership.Validators;

namespace FooCommerce.Infrastructure.Membership.Validators;

public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator(ILocationService locationService = null)
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.Email).SetValidator(new EmailValidator());
        RuleFor(x => x.Password).SetValidator(new PasswordValidator());

        if (locationService is not null)
            RuleFor(x => x.Country).SetValidator(new CountryValidator(locationService));
    }
}