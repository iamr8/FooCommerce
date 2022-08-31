using FluentValidation;

using FooCommerce.Application.Localization.Services;

namespace FooCommerce.MembershipAPI.Worker.Validators.PropertyValidators;

public class CountryValidator : AbstractValidator<uint>
{
    public CountryValidator(ILocationService locationService)
    {
        RuleFor(x => x).MustAsync(async (id, cancellation) =>
            await locationService.IsCountryValidAsync(id, cancellation));
    }
}