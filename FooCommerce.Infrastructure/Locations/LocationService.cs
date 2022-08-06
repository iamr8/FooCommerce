using System.Collections.Concurrent;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Entities.Listings;
using FooCommerce.Application.Services.Listings;
using FooCommerce.Domain.Services;
using FooCommerce.Infrastructure.Caching;
using FooCommerce.Infrastructure.Locations.Dtos;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Infrastructure.Locations;

public class LocationService : ILocationService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly IMemoryCacheService _cacheService;

    public LocationService(IDbContextFactory<AppDbContext> dbContextFactory, IMemoryCacheService cacheService)
    {
        _dbContextFactory = dbContextFactory;
        _cacheService = cacheService;
    }

    private async ValueTask<IEnumerable<LocationModel>> GetLocationsAsync(CancellationToken cancellationToken = default)
    {
        return await _cacheService.GetOrCreateAsync("locations", async () =>
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var query = from l in dbContext.Set<Location>()
                        where !l.IsDeleted && !l.IsHidden
                        orderby l.Division
                        select new
                        {
                            l.Id,
                            l.Division,
                            l.Name,
                            l.PublicId,
                            l.ParentId,
                        };
            var locations = await query.ToListAsync();
            if (locations is null or { Count: <= 0 })
                return default;

            var output = new ConcurrentDictionary<Guid, LocationModel>();
            foreach (var location in locations)
            {
                output.TryAdd(location.Id, new LocationModel(location.ParentId)
                {
                    Division = location.Division,
                    Name = location.Name,
                    PublicId = location.PublicId,
                });
            }

            foreach (var (id, locationModel) in output)
            {
                if (locationModel.ParentId is null)
                    continue;

                var hasParent = output.TryGetValue(locationModel.ParentId.Value, out var parent);
                if (!hasParent)
                    continue;

                parent.Children = parent.Children.Concat(new[] { locationModel });
                locationModel.Parent = parent;
            }

            var result = output
                .Select(x => x.Value)
                .OrderBy(x => x.Division)
                .AsEnumerable();
            return result;
        }, cancellationToken);
    }

    public async Task<bool> IsCountryValidAsync(uint countryId, CancellationToken cancellationToken = default)
    {
        var countries = await GetLocationsAsync(cancellationToken);
        return countries.Any(x => x.PublicId == countryId);
    }
}