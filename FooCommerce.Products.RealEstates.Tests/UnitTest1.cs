using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.RealEstates.Entities;

namespace FooCommerce.Products.RealEstates.Tests;

public class UnitTest1 : Fixture
{
    [Fact]
    public void EntityCreation()
    {
        // Arrange
        using var dbContext = new AppDbContext(this.DbContextOptions);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        // Act
        var product = dbContext.Set<RealEstate>().Add(new RealEstate()).Entity;
        var ad = dbContext.Set<RealEstateAd>().Add(new RealEstateAd
        {
            ProductId = product.Id,
            EndDate = DateTime.UtcNow.AddDays(30),
        }).Entity;

        dbContext.SaveChanges();
        var fetch = dbContext.Set<RealEstate>().First();

        Assert.NotNull(fetch);
    }
}