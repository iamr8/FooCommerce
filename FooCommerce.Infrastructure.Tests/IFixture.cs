using Autofac;

namespace FooCommerce.Infrastructure.Tests
{
    public interface IFixture
    {
        IContainer Container { get; }
    }
}