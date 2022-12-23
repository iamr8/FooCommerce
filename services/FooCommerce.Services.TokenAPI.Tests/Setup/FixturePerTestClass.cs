using FooCommerce.EventSource;
using FooCommerce.Tests;
using FooCommerce.TokenService.Sagas;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, DisableTestParallelization = true, MaxParallelThreads = 1)]

namespace FooCommerce.TokensService.Tests.Setup;

public class FixturePerTestClass : IAsyncLifetime
{
    public event OnLoggingEvent OnLogging;

    public LoggerFactory LoggerFactory { get; }

    public FixturePerTestClass()
    {
        LoggerFactory = new LoggerFactory();
        LoggerFactory.AddProvider(new ForwardingLoggerProvider((logLevel, category, eventId, message, exception) =>
            OnLogging?.Invoke(logLevel, category, eventId, message, exception)));

        // ConfigureServices
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables()
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<ILoggerFactory>(LoggerFactory);

        services.AddSingleton(sp => configuration);

        services.AddMassTransitTestHarness(cfg =>
        {
            cfg.ConfigureBus(config =>
            {
                config.BusConfig = configurator =>
                {
                    configurator
                        .AddSagaStateMachine<TokenStateMachine, TokenState>()
                        .InMemoryRepository();
                };
            });
        });

        // Configure
        ServiceProvider = services.BuildServiceProvider();

        LogContext.ConfigureCurrentLogContext(LoggerFactory);
        Harness = ServiceProvider.GetTestHarness();
        Harness.TestTimeout = TimeSpan.FromSeconds(5);
        Harness.TestInactivityTimeout = TimeSpan.FromSeconds(5);
    }

    public ServiceProvider ServiceProvider { get; }
    public ITestHarness Harness { get; }

    public async Task InitializeAsync()
    {
        await this.Harness.Start();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await this.Harness.Stop();
        await this.ServiceProvider.DisposeAsync();
    }
}