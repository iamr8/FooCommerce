using FluentValidation;

using FooCommerce.Application.Services.Listings;

namespace FooCommerce.Infrastructure.Membership.Validators.PropertyValidators;

public class CountryValidator : AbstractValidator<uint>
{
    public CountryValidator(ILocationService locationService)
    {
        RuleFor(x => x).MustAsync(async (id, cancellation) =>
            await locationService.IsCountryValidAsync(id, cancellation));
    }
}