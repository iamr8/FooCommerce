using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace FooCommerce.Infrastructure.Tests
{
    public static class FixtureExtensions
    {
        public static ILifetimeScope ConfigureLogging<TFixture>(this TFixture fixture, ITestOutputHelper outputHelper) where TFixture : class, IFixture
        {
            return fixture.Container.BeginLifetimeScope(containerBuilder =>
            {
                var services = new ServiceCollection();
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddXUnit(outputHelper);
                    loggingBuilder.AddDebug();
                    loggingBuilder.AddConsole();
                    loggingBuilder.AddEventLog();
                });
                containerBuilder.Populate(services);
            });
        }
    }
}