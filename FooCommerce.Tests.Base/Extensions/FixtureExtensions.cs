using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

using Xunit.Abstractions;

namespace FooCommerce.Tests.Extensions;

public static class FixtureExtensions
{
    public static IServiceScope ConfigureLogging<TFixture>(this TFixture fixture, ITestOutputHelper outputHelper) where TFixture : class, IFixture
    {
        var scope = fixture.ServiceProvider.CreateScope();

        var services = new ServiceCollection();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddXUnit(outputHelper);
            loggingBuilder.AddDebug();
            loggingBuilder.AddConsole();
            loggingBuilder.AddEventLog();
            loggingBuilder.AddConfiguration(scope.ServiceProvider.GetService<IConfiguration>());
            loggingBuilder.AddConfiguration();
        });
        var sc = services.BuildServiceProvider();
        var loggerFactory = sc.GetService<ILoggerFactory>();
        LogContext.ConfigureCurrentLogContext(loggerFactory);
        return scope;
    }
}