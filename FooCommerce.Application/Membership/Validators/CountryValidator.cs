using FluentValidation;

using FooCommerce.Application.Listings.Services;

namespace FooCommerce.Application.Membership.Validators;

public class CountryValidator : AbstractValidator<uint>
{
    public CountryValidator(ILocationService locationService)
    {
        RuleFor(x => x).MustAsync(async (id, cancellation) =>
            await locationService.IsCountryValidAsync(id, cancellation));
    }
}