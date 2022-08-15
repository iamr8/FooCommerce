using Dapper;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Dtos.Listings;
using FooCommerce.Application.Entities.Listings;
using FooCommerce.Application.Services.Listings;
using FooCommerce.Infrastructure.Caching;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FooCommerce.Infrastructure.Locations;

public class LocationService : ILocationService
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IMemoryCache _cacheService;
    private readonly ILogger<ILocationService> _logger;

    public LocationService(IDbConnectionFactory dbConnectionFactory, IMemoryCache cacheService, ILogger<ILocationService> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<LocationModel>> GetLocationsNonCachedAsync()
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();

        var locations = await dbConnection.QueryAsync<LocationModel>($"SELECT [location].{nameof(Location.Id)}, [location].{nameof(Location.Division)}, [location].{nameof(Location.Name)}, [location].{nameof(Location.PublicId)}, [location].{nameof(Location.ParentId)} " +
                                                              "FROM [Locations] AS [location] " +
                                                              $"WHERE [location].{nameof(Location.IsDeleted)} <> 1 AND [location].{nameof(Location.IsHidden)} <> 1");

        _logger.LogInformation("Locations are retrieved from database directly.");
        return locations;
    }

    public const string CacheKey = "config.locations";

    public async ValueTask<IEnumerable<LocationModel>> GetLocationsAsync(CancellationToken cancellationToken = default)
    {
        return await _cacheService.GetOrCreateAsync(CacheKey,
            async () => await GetLocationsNonCachedAsync(), _logger, cancellationToken);
    }

    public async Task<bool> IsCountryValidAsync(uint countryId, CancellationToken cancellationToken = default)
    {
        var countries = await GetLocationsAsync(cancellationToken);
        return countries.Any(x => x.PublicId == countryId);
    }
}