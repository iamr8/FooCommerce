using Autofac;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Services.Listings;
using FooCommerce.Infrastructure.Caching;
using FooCommerce.Infrastructure.Locations;
using FooCommerce.Infrastructure.Tests.Setups;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace FooCommerce.Infrastructure.Tests.LocationsTests;

public class LocationServiceTests : IClassFixture<Fixture>, ITestScope
{
    public Fixture Fixture { get; }
    public ITestOutputHelper TestConsole { get; }
    public ILifetimeScope Scope { get; }
    private LocationService LocationService;

    public LocationServiceTests(Fixture fixture, ITestOutputHelper outputHelper)
    {
        Fixture = fixture;
        TestConsole = outputHelper;
        Scope = fixture.ConfigureLogging(outputHelper);

        var memoryCache = Scope.Resolve<IMemoryCache>();
        var logger = Scope.Resolve<ILogger<ILocationService>>();
        var dbConnectionFactory = Scope.Resolve<IDbConnectionFactory>();
        memoryCache.Clear(LocationService.CacheKey);
        LocationService = new LocationService(dbConnectionFactory, memoryCache, logger);
    }

    [Fact]
    public async Task Should_Return_Locations_Cached()
    {
        // Arrange

        // Act
        var locations = await LocationService.GetLocationsAsync();

        // Assert
        Assert.NotNull(locations);
        Assert.NotEmpty(locations);
        Assert.Equal(5, locations.Count());
        Assert.Contains(locations, l => l.Division == LocationDivisions.Country && l.Name == "Iran");
        Assert.Contains(locations, l => l.Division == LocationDivisions.Province && l.Name == "Khuzestan");
        Assert.Contains(locations, l => l.Division == LocationDivisions.City && l.Name == "Ahvaz");
        Assert.Contains(locations, l => l.Division == LocationDivisions.District && l.Name == "Kianpars");
        Assert.Contains(locations, l => l.Division == LocationDivisions.Quarter && l.Name == "Western");
    }

    [Fact]
    public async Task Should_Validate_CountryId()
    {
        // Act
        var validCountry = await LocationService.IsCountryValidAsync(0);

        // Assert
        Assert.True(validCountry);
    }

    [Fact]
    public async Task Should_Not_Validate_CountryId()
    {
        // Act
        var validCountry = await LocationService.IsCountryValidAsync(1);

        // Assert
        Assert.False(validCountry);
    }
}