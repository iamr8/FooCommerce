using FooCommerce.Common.Helpers;
using FooCommerce.Infrastructure.Bootstrapper.Modules;
using FooCommerce.Tests;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

using Xunit.Abstractions;

namespace FooCommerce.Infrastructure.Tests.Setup;

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

        services.RegisterModule(new AutoFluentValidationModule());
        services.RegisterModule(new MvcModule());
        services.RegisterModule(new ServicesModule());
        services.RegisterModule(new ProtectionModule());

        return services.BuildServiceProvider();
    }

    public void Configure(IApplicationBuilder app)
    {
    }
}