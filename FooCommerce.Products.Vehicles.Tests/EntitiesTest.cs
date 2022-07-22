using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Vehicles.Entities;

namespace FooCommerce.Products.Vehicles.Tests;

public class EntitiesTest : Fixture
{
    [Fact]
    public void EntityCreation()
    {
        // Arrange
        using var dbContext = new AppDbContext(DbContextOptions);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        // Act
        var product = dbContext.Set<Vehicle>().Add(new Vehicle()).Entity;
        var ad = dbContext.Set<VehicleAd>().Add(new VehicleAd
        {
            ProductId = product.Id,
            EndDate = DateTime.UtcNow.AddDays(30),
        }).Entity;

        dbContext.SaveChanges();
        var fetch = dbContext.Set<Vehicle>().First();

        Assert.NotNull(fetch);
    }
}