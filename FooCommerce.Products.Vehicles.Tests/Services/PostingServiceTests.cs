using FooCommerce.Products.Ads.Services;
using FooCommerce.Products.Vehicles.Models;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Products.Vehicles.Tests.Services;

public class PostingServiceTests : Fixture
{
    [Fact]
    public async Task NewAsync()
    {
        // Arrange
        var mediator = ServiceProvider.GetService<IMediator>();
        var service = new PostingService(mediator);
        var request = new NewVehicleAdRequest();

        // Act
        var act = await service.NewAsync(request);

        // Assert
        Assert.True(act);
    }
}