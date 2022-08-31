using FooCommerce.Application.Listings.Dtos;

namespace FooCommerce.Application.Localization.Services;

public interface ILocationService
{
    ValueTask<IEnumerable<LocationModel>> GetLocationsAsync(CancellationToken cancellationToken = default);

    Task<bool> IsCountryValidAsync(uint countryId, CancellationToken cancellationToken = default);
}