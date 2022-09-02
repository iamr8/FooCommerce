using Autofac;

using EasyCaching.Core;

using FooCommerce.Domain.DbProvider;
using FooCommerce.Infrastructure.Locations.Enums;
using FooCommerce.Infrastructure.Services;
using FooCommerce.Infrastructure.Services.Repositories;
using FooCommerce.Infrastructure.Tests.Setups;
using FooCommerce.Tests;
using FooCommerce.Tests.Extensions;

using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace FooCommerce.Infrastructure.Tests.LocationsTests;

public class LocationServiceTests : IClassFixture<Fixture>, ITestScope<Fixture>
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

        var easyCaching = Scope.Resolve<IEasyCachingProvider>();
        var logger = Scope.Resolve<ILogger<ILocationService>>();
        var dbConnectionFactory = Scope.Resolve<IDbConnectionFactory>();
        easyCaching.Flush();
        LocationService = new LocationService(dbConnectionFactory, easyCaching, logger);
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
        Assert.Contains(locations, l => l.Division == LocationDivision.Country && l.Name == "Iran");
        Assert.Contains(locations, l => l.Division == LocationDivision.Province && l.Name == "Khuzestan");
        Assert.Contains(locations, l => l.Division == LocationDivision.City && l.Name == "Ahvaz");
        Assert.Contains(locations, l => l.Division == LocationDivision.District && l.Name == "Kianpars");
        Assert.Contains(locations, l => l.Division == LocationDivision.Quarter && l.Name == "Western");
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