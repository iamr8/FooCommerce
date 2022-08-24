using Autofac;

namespace FooCommerce.Tests.Base;

public interface IFixture
{
    IContainer Container { get; }
}