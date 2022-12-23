using FooCommerce.EventSource;
using FooCommerce.NotificationService.Consumers;
using FooCommerce.NotificationService.Contracts;
using FooCommerce.NotificationService.DbProvider;
using FooCommerce.NotificationService.Handlers;
using FooCommerce.NotificationService.Interfaces;
using FooCommerce.NotificationService.Models;
using FooCommerce.NotificationService.Services;
using FooCommerce.NotificationService.Services.Repositories;
using FooCommerce.Tests;
using FooCommerce.Tests.Mocks;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, DisableTestParallelization = true, MaxParallelThreads = 1)]

namespace FooCommerce.NotificationService.Tests.Setup;

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

        foreach (var emailClient in configuration.GetSection("Emails").Get<EmailClient[]>())
        {
            services.AddSingleton<IEmailClient, EmailClient>(_ => emailClient);
        }

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddDebug();
            loggingBuilder.AddConsole();
            loggingBuilder.AddEventLog();
            loggingBuilder.AddConfiguration();
        });
        services.AddSingleton(_ => MockObjects.GetWebHostEnvironment());

        services.AddDbContextFactory<NotificationDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));

        services.AddLocalizerTesting();

        services.AddScoped<EnqueueEmailConsumer>();
        services.AddScoped<EnqueuePushConsumer>();
        services.AddScoped<EnqueueSmsConsumer>();

        services.AddMassTransitTestHarness(cfg =>
        {
            cfg.ConfigureBus(config =>
            {
                config.TransportConfig = (_, context) =>
                {
                    context.Message<EnqueueEmail>(c => c.SetEntityName("notification"));
                    context.Message<EnqueueSms>(c => c.SetEntityName("notification"));
                    context.Message<EnqueuePush>(c => c.SetEntityName("notification"));
                };
                config.BusConfig = configurator =>
                {
                    configurator.AddConsumer<EnqueueEmailConsumer>();
                    configurator.AddConsumer<EnqueueSmsConsumer>();
                    configurator.AddConsumer<EnqueuePushConsumer>();
                };
            });
        });

        services.AddScoped<IHandler, EmailHandler>();
        services.AddScoped<IHandler, PushHandler>();
        services.AddScoped<IHandler, SmsHandler>();

        services.AddScoped<ICoordinator, Coordinator>();
        services.AddScoped<ITemplateService, TemplateService>();

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