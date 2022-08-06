namespace FooCommerce.Application.Services.Listings;

public interface ILocationService
{
    Task<bool> IsCountryValidAsync(uint countryId, CancellationToken cancellationToken = default);
}