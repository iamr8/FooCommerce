using FooCommerce.Common.Helpers;
using FooCommerce.IdentityAPI.Worker.Modules;
using FooCommerce.Tests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Xunit.Abstractions;

namespace FooCommerce.IdentityAPI.Worker.Tests.Setup;

public class TestStartup : IStartup
{
    public readonly IConfiguration Configuration;
    public readonly string ConnectionString;
    public readonly ITestOutputHelper TestOutputHelper;

    public TestStartup(IConfiguration configuration, string connectionString, ITestOutputHelper testOutputHelper)
    {
        Configuration = configuration;
        ConnectionString = connectionString;
        TestOutputHelper = testOutputHelper;
    }

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddXUnit(TestOutputHelper);
            loggingBuilder.AddDebug();
            loggingBuilder.AddConsole();
            loggingBuilder.AddEventLog();
            loggingBuilder.AddConfiguration();
        });
        services.AddSingleton(_ => MockObjects.GetHostEnvironment());
        services.AddSingleton(_ => MockObjects.GetWebHostEnvironment());
        services.AddSingleton(_ => Configuration);

        services.AddSingleton(_ => MockObjects.GetWebHostEnvironment());
        services.RegisterModule(new LocalizationModule());
        services.RegisterModule(new TestBusModule());
        services.RegisterModule(new ServicesModule());
        services.RegisterModule(new CacheProviderModule());
        services.RegisterModule(new AppDatabaseProviderModule(ConnectionString, optionsBuilder =>
        {
            optionsBuilder.UseSqlServer(ConnectionString!,
                config =>
                {
                    config.EnableRetryOnFailure(3);
                    config.UseNetTopologySuite();
                    config.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
        }));

        return services.BuildServiceProvider();
    }

    public void Configure(IApplicationBuilder app)
    {
    }
}