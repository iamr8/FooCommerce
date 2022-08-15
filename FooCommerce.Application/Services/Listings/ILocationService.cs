using FooCommerce.Application.Dtos.Listings;

namespace FooCommerce.Application.Services.Listings;

public interface ILocationService
{
    ValueTask<IEnumerable<LocationModel>> GetLocationsAsync(CancellationToken cancellationToken = default);

    Task<bool> IsCountryValidAsync(uint countryId, CancellationToken cancellationToken = default);
}