using FooCommerce.Common.Helpers;
using FooCommerce.IdentityAPI.Worker;
using FooCommerce.IdentityAPI.Worker.Modules;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Configuration;

var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(loggerFactory =>
    {
        loggerFactory
            .AddConsole()
            .AddEventLog()
            .AddEventSourceLogger()
            .AddDebug()
            .AddConfiguration();
    })
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddService<BusModule>();
        services.AddService<CacheProviderModule>();
        services.AddService<ServicesModule>();
        services.AddService(new AppDatabaseProviderModule(connectionString, optionsBuilder =>
        {
            optionsBuilder.UseSqlServer(connectionString!,
                config =>
                {
                    config.EnableRetryOnFailure(3);
                    config.CommandTimeout(3);
                    config.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
        }));
    })
    .Build();

await host.RunAsync();