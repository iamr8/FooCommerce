using Autofac;

using FooCommerce.NotificationAPI.Worker.Services;
using FooCommerce.Tests;
using FooCommerce.Tests.Extensions;

using Xunit.Abstractions;

namespace FooCommerce.NotificationAPI.Worker.Tests;

public class NotificationClientServiceTests : IClassFixture<Fixture>, ITestScope<Fixture>
{
    public Fixture Fixture { get; }
    public ITestOutputHelper TestConsole { get; }
    public ILifetimeScope Scope { get; }

    public NotificationClientServiceTests(Fixture fixture, ITestOutputHelper outputHelper)
    {
        this.Fixture = fixture;
        this.TestConsole = outputHelper;
        this.Scope = fixture.ConfigureLogging(outputHelper);
    }

    [Fact]
    public void Should_Return_Clients()
    {
        // Arrange
        var service = this.Scope.Resolve<INotificationClientService>();

        // Act
        var clients = service.GetAvailableMailboxCredentials();

        // Assert
        Assert.NotNull(clients);
        Assert.NotEmpty(clients);

        Assert.Single(clients);

        Assert.Contains(clients, client => client.SenderAddress == "noreply@foocommerce.com");
    }
}