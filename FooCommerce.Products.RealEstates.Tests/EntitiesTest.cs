using Autofac;

using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.RealEstates.Entities;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Products.RealEstates.Tests;

public class EntitiesTest : Fixture
{
    [Fact]
    public async Task EntityCreationAsync()
    {
        // Arrange
        var dbContextFactory = this.Container.Resolve<IDbContextFactory<AppDbContext>>();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        // Act
        var product = dbContext.Set<RealEstate>().Add(new RealEstate()).Entity;
        var ad = dbContext.Set<RealEstateAd>().Add(new RealEstateAd
        {
            ProductId = product.Id,
            EndDate = DateTime.UtcNow.AddDays(30),
        }).Entity;

        await dbContext.SaveChangesAsync();
        var fetchedProduct = dbContext.Set<RealEstate>().First();
        var fetchedAd = dbContext.Set<RealEstateAd>().First();

        Assert.NotNull(fetchedProduct);
        Assert.NotNull(fetchedAd);
        Assert.Equal(product.Id, fetchedProduct.Id);
        Assert.Equal(ad.ProductId, fetchedAd.ProductId);
    }
}