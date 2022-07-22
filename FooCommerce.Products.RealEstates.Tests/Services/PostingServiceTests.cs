using FooCommerce.Products.Ads.Services;
using FooCommerce.Products.RealEstates.Application.Models;

namespace FooCommerce.Products.RealEstates.Tests.Services;

public class PostingServiceTests : Fixture
{
    [Fact]
    public async Task NewAsync()
    {
        // Arrange
        var service = new PostingService(this.Mediator);
        var request = new NewRealEstateAdRequest();

        // Act
        var act = await service.NewAsync(request);

        // Assert
        Assert.True(act);
    }
}