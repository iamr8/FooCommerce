using FooCommerce.Domain;
using FooCommerce.Domain.DbProvider;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Products.Tests
{
    public class Fixture
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly DbContextOptions<AppDbContext> DbContextOptions;

        protected Fixture()
        {
            var assemblies = AppDomain.CurrentDomain.GetExecutingAssemblies().ToArray();
            var services = new ServiceCollection();
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("ProductsInMemoryDatabase")
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .EnableThreadSafetyChecks();
            services.AddPooledDbContextFactory<AppDbContext>(optionsBuilder =>
            {
                optionsBuilder = dbContextOptionsBuilder;
            });
            services.AddMediatR(assemblies);

            DbContextOptions = dbContextOptionsBuilder.Options;
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}