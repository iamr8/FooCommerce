using Autofac.Extensions.DependencyInjection;

using FluentValidation.AspNetCore;

using FooCommerce.Application.DbProvider;
using FooCommerce.Infrastructure.Shopping.Contracts;
using FooCommerce.Infrastructure.Shopping.StateMachines;

using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(c => c.Populate(builder.Services)));

builder.Configuration.AddJsonFile("appsettings.json", true, true);
if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
{
    var path = $"appsettings.{builder.Environment.EnvironmentName}.json";
    builder.Configuration.AddJsonFile(path, true, true);
}
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddMassTransit(configurator =>
{
    configurator.AddSagaStateMachine<OrderStateMachine, OrderState>()
        .EntityFrameworkRepository(r =>
        {
            r.ExistingDbContext<AppDbContext>();
            r.UseSqlServer();
        });

    configurator.AddRequestClient<AcceptOrder>();
    configurator.AddRequestClient<GetOrder>();

    configurator.UsingInMemory((context, cfg) =>
    {
        cfg.AutoStart = true;
        cfg.ConfigureEndpoints(context);
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

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