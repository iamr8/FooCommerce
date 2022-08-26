using Autofac;

namespace FooCommerce.Tests;

public interface IFixture
{
    IContainer Container { get; }
}