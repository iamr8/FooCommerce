using FooCommerce.Products.Domain.Interfaces;
using FooCommerce.Products.RealEstates.Commands;
using FooCommerce.Products.RealEstates.Models;

using MassTransit.Testing;

namespace FooCommerce.Products.RealEstates.Tests.Services;

public class PostingServiceTests : Fixture
{
    [Fact]
    public async Task NewAsync()
    {
        // Arrange
        var request = new NewRealEstateAdRequest();

        // Act
        await this.TestHarness.Start();
        var client = this.TestHarness.GetRequestClient<NewRealEstateAdRequest>();

        var response1 = await client.GetResponse<IAdRequestResponse>(request);
        dynamic response2 = this.TestHarness.Sent.Select<IAdRequestResponse>().First().MessageObject;

        // Assert
        Assert.True(await this.TestHarness.Sent.Any<IAdRequestResponse>());
        Assert.True(await this.TestHarness.Consumed.Any<NewRealEstateAdRequest>());
        Assert.True(await this.TestHarness.GetConsumerHarness<CreateRealEstateAdConsumer>().Consumed.Any<NewRealEstateAdRequest>());
        Assert.NotNull(response1.Message);
        Assert.NotNull(response2);
        Assert.True(response2.IsSuccess);

        await this.TestHarness.Stop();
    }
}