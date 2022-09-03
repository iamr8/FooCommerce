using Dapper;

using EasyCaching.Core;

using FooCommerce.Common.Caching;
using FooCommerce.Domain.DbProvider;
using FooCommerce.Infrastructure.DbProvider.Entities.Configurations;
using FooCommerce.Infrastructure.Listings.Dtos;

using Microsoft.Extensions.Logging;

namespace FooCommerce.Infrastructure.Services.Repositories;

public class LocationService : ILocationService
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IEasyCachingProvider _cachingProvider;
    private readonly ILogger<ILocationService> _logger;

    public LocationService(IDbConnectionFactory dbConnectionFactory, IEasyCachingProvider cachingProvider, ILogger<ILocationService> logger)
    {
        _cachingProvider = cachingProvider;
        _logger = logger;
        _dbConnectionFactory = dbConnectionFactory;
    }

    private async Task<IEnumerable<LocationModel>> GetLocationsNonCachedAsync()
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();

        var locations = await dbConnection.QueryAsync<LocationModel>($"SELECT [location].{nameof(Location.Id)}, [location].{nameof(Location.Division)}, [location].{nameof(Location.Name)}, [location].{nameof(Location.PublicId)}, [location].{nameof(Location.ParentId)} " +
                                                              "FROM [Locations] AS [location] " +
                                                              $"WHERE [location].{nameof(Location.IsDeleted)} <> 1 AND [location].{nameof(Location.IsHidden)} <> 1");

        _logger.LogInformation("Locations are retrieved from database directly.");
        return locations;
    }

    private const string CacheKey = "config.locations";

    public async ValueTask<IEnumerable<LocationModel>> GetLocationsAsync(CancellationToken cancellationToken = default)
    {
        return await _cachingProvider.GetOrCreateAsync(CacheKey,
            async () => await GetLocationsNonCachedAsync(), _logger, cancellationToken);
    }

    public async Task<bool> IsCountryValidAsync(uint countryId, CancellationToken cancellationToken = default)
    {
        var countries = await GetLocationsAsync(cancellationToken);
        return countries.Any(x => x.PublicId == countryId);
    }
}