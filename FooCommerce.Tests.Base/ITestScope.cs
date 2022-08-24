using Autofac;

using Xunit.Abstractions;

namespace FooCommerce.Tests.Base;

public interface ITestScope<out T> where T : class
{
    T Fixture { get; }
    ITestOutputHelper TestConsole { get; }
    ILifetimeScope Scope { get; }
}