using FooCommerce.Infrastructure.Listings.Dtos;

namespace FooCommerce.Infrastructure.Services;

public interface ILocationService
{
    ValueTask<IEnumerable<LocationModel>> GetLocationsAsync(CancellationToken cancellationToken = default);

    Task<bool> IsCountryValidAsync(uint countryId, CancellationToken cancellationToken = default);
}