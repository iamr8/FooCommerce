using Autofac;

using FooCommerce.Domain;
using FooCommerce.Domain.Modules;

using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Products.RealEstates.Tests;

public class Fixture
{
    protected readonly IContainer Container;

    protected readonly IMediator Mediator;

    protected Fixture()
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterModule(
            new DbContextModule(builder =>
                builder.UseInMemoryDatabase("ProductsInMemoryDatabase")));
        containerBuilder.RegisterMediatR(AppDomain.CurrentDomain.GetExecutingAssemblies().ToArray());
        containerBuilder.RegisterModule<FooCommerce.Products.PluginModule>();

        Container = containerBuilder.Build();
        Mediator = Container.Resolve<IMediator>();
    }
}