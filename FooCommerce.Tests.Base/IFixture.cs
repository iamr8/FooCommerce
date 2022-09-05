namespace FooCommerce.Tests;

public interface IFixture : IAsyncDisposable
{
    IServiceProvider ServiceProvider { get; }

    Task InitializeAsync();
}