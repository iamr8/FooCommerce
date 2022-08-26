using Autofac;

using Xunit.Abstractions;

namespace FooCommerce.Tests;

public interface ITestScope<out T> where T : class
{
    T Fixture { get; }
    ITestOutputHelper TestConsole { get; }
    ILifetimeScope Scope { get; }
}