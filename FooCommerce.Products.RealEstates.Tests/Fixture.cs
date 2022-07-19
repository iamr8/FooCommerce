using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Products.RealEstates.Tests
{
    public class Fixture
    {
        protected readonly DbContextOptions<AppDbContext> DbContextOptions;

        public Fixture()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("RealEstateInMemoryDatabase")
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .EnableThreadSafetyChecks();
            DbContextOptions = dbContextOptionsBuilder.Options;
        }
    }
}