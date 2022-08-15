using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Application.Modules;
using FooCommerce.Infrastructure.Modules;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", true, true);
if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
{
    var path = $"appsettings.{builder.Environment.EnvironmentName}.json";
    builder.Configuration.AddJsonFile(path, true, true);
}
builder.Configuration.AddEnvironmentVariables();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new AutoFluentValidationModule());
        containerBuilder.RegisterModule(new MvcModule());
        containerBuilder.RegisterModule(new EventBusModule());
        containerBuilder.RegisterModule(new CachingModule());
        containerBuilder.RegisterModule(new DapperModule(connectionString));
        containerBuilder.RegisterModule(new DbContextModule(config =>
        {
            config.UseSqlServer(connectionString,
                config =>
                {
                    config.EnableRetryOnFailure(3);
                    config.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    //config.ExecutionStrategy(dependencies =>
                    //{
                    //    dependencies.Logger.
                    //})
                });
        }));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html"); ;

app.Run();