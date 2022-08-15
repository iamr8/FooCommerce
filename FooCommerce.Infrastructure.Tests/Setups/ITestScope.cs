using Autofac;

using Xunit.Abstractions;

namespace FooCommerce.Infrastructure.Tests.Setups;

public interface ITestScope
{
    Fixture Fixture { get; }
    ITestOutputHelper TestConsole { get; }
    ILifetimeScope Scope { get; }
}