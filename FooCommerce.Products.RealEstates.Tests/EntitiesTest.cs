using Autofac;

using FooCommerce.Application.DbProvider;
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
        var ad = dbContext.Set<RealEstate>().Add(new RealEstate
        {
            EndDate = DateTime.UtcNow.AddDays(30),
        }).Entity;

        await dbContext.SaveChangesAsync();
        var fetchedAd = dbContext.Set<RealEstate>().First();

        Assert.NotNull(fetchedAd);
        Assert.Equal(ad.Id, fetchedAd.Id);
    }
}