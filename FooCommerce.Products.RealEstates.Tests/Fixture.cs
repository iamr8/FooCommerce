using Autofac;

using FooCommerce.Domain;
using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.Modules;

using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Products.RealEstates.Tests
{
    public class Fixture
    {
        protected readonly IContainer Container;

        protected readonly DbContextOptions<AppDbContext> DbContextOptions;

        protected readonly IMediator Mediator;

        protected Fixture()
        {
            var containerBuilder = new ContainerBuilder();

            var assemblies = AppDomain.CurrentDomain.GetSolutionAssemblies().ToArray();
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("ProductsInMemoryDatabase")
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .EnableThreadSafetyChecks();

            containerBuilder.RegisterModule(new DbContextModule(dbContextOptionsBuilder, null));
            containerBuilder.RegisterMediatR(assemblies);
            containerBuilder.RegisterModule<FooCommerce.Products.PluginModule>();

            Container = containerBuilder.Build();
            DbContextOptions = dbContextOptionsBuilder.Options;
            Mediator = Container.Resolve<IMediator>();
        }
    }
}