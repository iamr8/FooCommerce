using Autofac;

using Xunit.Abstractions;

namespace FooCommerce.Infrastructure.Tests
{
    public interface ITest
    {
        Fixture Fixture { get; }
        ITestOutputHelper TestConsole { get; }
        ILifetimeScope Scope { get; }
    }
}