using Autofac;

using FooCommerce.Products.RealEstates.Models;
using FooCommerce.Products.Services;

namespace FooCommerce.Products.RealEstates.Tests.Services;

public class PostingServiceTests : Fixture
{
    [Fact]
    public async Task NewAsync()
    {
        // Arrange
        var service = this.Container.Resolve<IPostingService>();
        var request = new NewRealEstateAdRequest();

        // Act
        var act = await service.NewAsync(request);

        // Assert
        Assert.True(act);
    }
}