using FluentValidation;

using FooCommerce.Application.Services.Listings;

namespace FooCommerce.Application.Validators;

public class CountryValidator : AbstractValidator<uint>
{
    public CountryValidator(ILocationService locationService)
    {
        RuleFor(x => x).MustAsync(async (id, cancellation) =>
            await locationService.IsCountryValidAsync(id, cancellation));
    }
}