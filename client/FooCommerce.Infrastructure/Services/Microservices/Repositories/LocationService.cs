//using Dapper;

//using FooCommerce.Caching;
//using FooCommerce.DbProvider;
//using FooCommerce.DbProvider.Entities.Configurations;
//using FooCommerce.DbProvider.Entities.Listings.Dtos;

//using Microsoft.Extensions.Logging;

//namespace FooCommerce.Infrastructure.Services.Repositories;

//public class LocationService : ILocationService
//{
//    private readonly IDbConnectionFactory _dbConnectionFactory;
//    private readonly ICacheProvider _cacheProvider;
//    private readonly ILogger<ILocationService> _logger;

//    public LocationService(IDbConnectionFactory dbConnectionFactory, ICacheProvider cacheProvider, ILogger<ILocationService> logger)
//    {
//        _cacheProvider = cacheProvider;
//        _logger = logger;
//        _dbConnectionFactory = dbConnectionFactory;
//    }

//    private async Task<IEnumerable<LocationModel>> GetLocationsNonCachedAsync()
//    {
//        using (var dbConnection = _dbConnectionFactory.CreateConnection())
//        {
//            var locations = await dbConnection.QueryAsync<LocationModel>(
//                $"SELECT [location].{nameof(Location.Id)}, [location].{nameof(Location.Division)}, [location].{nameof(Location.Name)}, [location].{nameof(Location.ExternalId)}, [location].{nameof(Location.ParentId)} " +
//                     "FROM [Locations] AS [location] " +
//                    $"WHERE [location].{nameof(Location.IsDeleted)} <> 1 AND [location].{nameof(Location.IsInvisible)} <> 1");

//            _logger.LogInformation("Locations are retrieved from database directly.");
//            return locations;
//        }
//    }

//    private const string CacheKey = "config.locations";

//    public async ValueTask<IEnumerable<LocationModel>> GetLocationsAsync(CancellationToken cancellationToken = default)
//    {
//        return await _cacheProvider.GetOrCreateAsync(CacheKey,
//            async _ => await GetLocationsNonCachedAsync(),
//            new CacheOptions { Logger = _logger },
//            cancellationToken);
//    }

//    public async Task<bool> IsCountryValidAsync(uint countryId, CancellationToken cancellationToken = default)
//    {
//        var countries = await GetLocationsAsync(cancellationToken);
//        return countries.Any(x => x.ExternalId == countryId);
//    }
//}