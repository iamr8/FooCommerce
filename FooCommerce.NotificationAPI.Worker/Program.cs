using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.NotificationAPI.Worker.Modules;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", true, true);
if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
{
    var path = $"appsettings.{builder.Environment.EnvironmentName}.json";
    builder.Configuration.AddJsonFile(path, true, true);
}
builder.Configuration.AddEnvironmentVariables();

var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new MvcModule());
        containerBuilder.RegisterModule(new BusModule());
        containerBuilder.RegisterModule(new CachingModule());
        containerBuilder.RegisterModule(new ProtectionModule());
        containerBuilder.RegisterModule(new DatabaseProviderModule(connectionString, optionsBuilder =>
        {
            optionsBuilder.UseSqlServer(connectionString!,
                config =>
                {
                    config.EnableRetryOnFailure(3);
                    config.CommandTimeout(3);
                    config.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
        }));
    });

var app = builder.Build();

app.Run();