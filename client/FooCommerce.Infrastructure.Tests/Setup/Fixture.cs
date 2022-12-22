//using EasyCaching.Core;

//using FooCommerce.DbProvider;
//using FooCommerce.DbProvider.Entities.Configurations;
//using FooCommerce.Infrastructure.Locations.Enums;
//using FooCommerce.Tests;

//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//using Xunit.Abstractions;

//namespace FooCommerce.Infrastructure.Tests.Setup;

//public class Fixture : IFixture
//{
//    public ServiceProvider ServiceProvider { get; private set; }
//    private readonly ITestOutputHelper _output;
//    private string connectionString;

//    public Fixture(ITestOutputHelper output)
//    {
//        _output = output;
//    }

//    public async Task InitializeAsync()
//    {
//        // ConfigureServices
//        var configuration = new ConfigurationBuilder()
//            .SetBasePath(Directory.GetCurrentDirectory())
//            .AddJsonFile("appsettings.json", false, true)
//            .AddEnvironmentVariables()
//            .Build();
//        connectionString = configuration.GetConnectionString("Default");

//        var testStartup = new TestStartup(configuration, connectionString, _output);
//        var services = new ServiceCollection();

//        // Configure
//        ServiceProvider = (ServiceProvider)testStartup.ConfigureServices(services);

//        await DatabaseCheckpoint.checkpoint.Reset(connectionString);

//        var dbContextFactory = ServiceProvider.GetService<IDbContextFactory<AppDbContext>>();
//        var dbContext = await dbContextFactory.CreateDbContextAsync();
//        AppDbContext.TestMode = true;
//        SeedLocationsData(dbContext);
//    }

//    private void SeedLocationsData(AppDbContext dbContext)
//    {
//        var country = dbContext.Locations.Add(new Location
//        {
//            Division = (byte)LocationDivision.Country,
//            Name = "Iran",
//        }).Entity;
//        var province = dbContext.Locations.Add(new Location
//        {
//            Division = (byte)LocationDivision.Province,
//            Name = "Khuzestan",
//            ParentId = country.Id,
//        }).Entity;
//        var city = dbContext.Locations.Add(new Location
//        {
//            Division = (byte)LocationDivision.City,
//            Name = "Ahvaz",
//            ParentId = province.Id,
//        }).Entity;
//        var district = dbContext.Locations.Add(new Location
//        {
//            Division = (byte)LocationDivision.District,
//            Name = "Kianpars",
//            ParentId = city.Id,
//        }).Entity;
//        var neighborhood = dbContext.Locations.Add(new Location
//        {
//            Division = (byte)LocationDivision.Quarter,
//            Name = "Western",
//            ParentId = district.Id,
//        }).Entity;
//        dbContext.SaveChanges();
//    }

//    public async ValueTask DisposeAsync()
//    {
//        await DatabaseCheckpoint.checkpoint.Reset(connectionString);
//        var cachingProvider = ServiceProvider.GetService<IEasyCachingProvider>();
//        await cachingProvider.FlushAsync();
//        GC.SuppressFinalize(this);
//    }
//}