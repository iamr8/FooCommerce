using Autofac;
using Autofac.Extensions.DependencyInjection;

using MassTransit;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace FooCommerce.Infrastructure.Tests.Setups
{
    public static class FixtureExtensions
    {
        public static ILifetimeScope ConfigureLogging<TFixture>(this TFixture fixture, ITestOutputHelper outputHelper) where TFixture : class, IFixture
        {
            var scope = fixture.Container.BeginLifetimeScope(containerBuilder =>
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

            var loggerFactory = scope.Resolve<ILoggerFactory>();
            LogContext.ConfigureCurrentLogContext(loggerFactory);

            return scope;
        }
    }
}