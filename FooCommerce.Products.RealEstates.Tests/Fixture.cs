using Autofac;

using FooCommerce.Domain.Modules;
using FooCommerce.Products.RealEstates.Commands;

using MassTransit.Testing;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Products.RealEstates.Tests;

public class Fixture
{
    protected readonly IContainer Container;
    protected readonly ITestHarness TestHarness;

    protected Fixture()
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterModule(new DatabaseModule(builder => builder.UseInMemoryDatabase("ProductsInMemoryDatabase")));
        containerBuilder.RegisterModule(new BusModule(true, typeof(CreateRealEstateAdConsumer).Assembly));
        containerBuilder.RegisterModule(new ProductModule());

        Container = containerBuilder.Build();
        TestHarness = this.Container.Resolve<ITestHarness>();
    }
}