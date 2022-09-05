using Microsoft.Extensions.DependencyInjection;

using Xunit.Abstractions;

namespace FooCommerce.Tests;

public interface ITestScope<out T> where T : class
{
    T Fixture { get; }
    ITestOutputHelper TestConsole { get; }
    //IServiceScope Scope { get; }
}