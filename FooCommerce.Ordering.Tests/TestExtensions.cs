using MassTransit;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FooCommerce.Ordering.Tests;

public static class TestExtensions
{
    public static void ConfigureLogging(this IServiceProvider provider)
    {
        var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

        LogContext.ConfigureCurrentLogContext(loggerFactory);
    }
}