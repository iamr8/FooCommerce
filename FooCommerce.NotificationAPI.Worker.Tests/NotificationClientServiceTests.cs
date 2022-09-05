using FooCommerce.NotificationAPI.Worker.Services;
using FooCommerce.NotificationAPI.Worker.Tests.Setup;

using Microsoft.Extensions.DependencyInjection;

using Xunit.Abstractions;

namespace FooCommerce.NotificationAPI.Worker.Tests;

public class NotificationClientServiceTests
{
    public ITestOutputHelper TestConsole { get; }

    public NotificationClientServiceTests(ITestOutputHelper outputHelper)
    {
        TestConsole = outputHelper;
    }

    [Fact]
    public async Task Should_Return_Clients()
    {
        // Arrange
        await using var fixture = new Fixture(TestConsole);
        await fixture.InitializeAsync();
        var service = fixture.ServiceProvider.GetService<INotificationClientService>();

        // Act
        var clients = service.GetAvailableMailboxCredentials();

        // Assert
        Assert.NotNull(clients);
        Assert.NotEmpty(clients);

        Assert.Single(clients);

        Assert.Contains(clients, client => client.SenderAddress == "noreply@foocommerce.com");
    }
}