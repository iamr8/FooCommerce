using Autofac;

using FooCommerce.Application.Modules;

using MassTransit.Testing;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Products.PurchasableItems.Tests;

public class Fixture
{
    protected readonly IContainer Container;
    protected readonly ITestHarness TestHarness;

    protected Fixture()
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterModule(new DatabaseModule(builder => builder.UseInMemoryDatabase("ProductsInMemoryDatabase")));
        containerBuilder.RegisterModule(new BusModule(true, typeof(ProductPurchasableItemsModule).Assembly));
        containerBuilder.RegisterModule(new ProductModule());

        Container = containerBuilder.Build();
        TestHarness = Container.Resolve<ITestHarness>();
    }
}