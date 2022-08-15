using Autofac;

namespace FooCommerce.Infrastructure.Tests.Setups
{
    public interface IFixture
    {
        IContainer Container { get; }
    }
}