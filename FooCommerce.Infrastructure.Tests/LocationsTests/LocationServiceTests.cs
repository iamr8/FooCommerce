using FooCommerce.Caching;
using FooCommerce.DbProvider;
using FooCommerce.Infrastructure.Locations.Enums;
using FooCommerce.Infrastructure.Services;
using FooCommerce.Infrastructure.Services.Repositories;
using FooCommerce.Infrastructure.Tests.Setup;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace FooCommerce.Infrastructure.Tests.LocationsTests;

public class LocationServiceTests
{
    public ITestOutputHelper TestConsole { get; }

    public LocationServiceTests(ITestOutputHelper outputHelper)
    {
        TestConsole = outputHelper;
    }

    [Fact]
    public async Task Should_Return_Locations_Cached()
    {
        // Arrange
        await using var fixture = new Fixture(TestConsole);
        await fixture.InitializeAsync();
        var cacheProvider = fixture.ServiceProvider.GetService<ICacheProvider>();
        var logger = fixture.ServiceProvider.GetService<ILogger<ILocationService>>();
        var dbConnectionFactory = fixture.ServiceProvider.GetService<IDbConnectionFactory>();
        await cacheProvider.FlushAsync();
        var locationService = new LocationService(dbConnectionFactory, cacheProvider, logger);

        // Act
        var locations = await locationService.GetLocationsAsync();

        // Assert
        Assert.NotNull(locations);
        Assert.NotEmpty(locations);
        Assert.Equal(5, locations.Count());
        Assert.Contains(locations, l => l.Division == (byte)LocationDivision.Country && l.Name == "Iran");
        Assert.Contains(locations, l => l.Division == (byte)LocationDivision.Province && l.Name == "Khuzestan");
        Assert.Contains(locations, l => l.Division == (byte)LocationDivision.City && l.Name == "Ahvaz");
        Assert.Contains(locations, l => l.Division == (byte)LocationDivision.District && l.Name == "Kianpars");
        Assert.Contains(locations, l => l.Division == (byte)LocationDivision.Quarter && l.Name == "Western");
    }

    [Fact]
    public async Task Should_Validate_CountryId()
    {
        // Arrange
        await using var fixture = new Fixture(TestConsole);
        await fixture.InitializeAsync();
        var cacheProvider = fixture.ServiceProvider.GetService<ICacheProvider>();
        var logger = fixture.ServiceProvider.GetService<ILogger<ILocationService>>();
        var dbConnectionFactory = fixture.ServiceProvider.GetService<IDbConnectionFactory>();
        await cacheProvider.FlushAsync();
        var locationService = new LocationService(dbConnectionFactory, cacheProvider, logger);

        // Act
        var validCountry = await locationService.IsCountryValidAsync(0);

        // Assert
        Assert.True(validCountry);
    }

    [Fact]
    public async Task Should_Not_Validate_CountryId()
    {
        // Arrange
        await using var fixture = new Fixture(TestConsole);
        await fixture.InitializeAsync();
        var cacheProvider = fixture.ServiceProvider.GetService<ICacheProvider>();
        var logger = fixture.ServiceProvider.GetService<ILogger<ILocationService>>();
        var dbConnectionFactory = fixture.ServiceProvider.GetService<IDbConnectionFactory>();
        await cacheProvider.FlushAsync();
        var locationService = new LocationService(dbConnectionFactory, cacheProvider, logger);

        // Act
        var validCountry = await locationService.IsCountryValidAsync(1);

        // Assert
        Assert.False(validCountry);
    }
}