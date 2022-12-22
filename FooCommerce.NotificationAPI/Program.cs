using FooCommerce.EventSource;
using FooCommerce.Localization.DependencyInjection;
using FooCommerce.Services.NotificationAPI.Consumers;
using FooCommerce.Services.NotificationAPI.DbProvider;
using FooCommerce.Services.NotificationAPI.Handlers;
using FooCommerce.Services.NotificationAPI.Interfaces;
using FooCommerce.Services.NotificationAPI.Models;
using FooCommerce.Services.NotificationAPI.Services;
using FooCommerce.Services.NotificationAPI.Services.Repositories;

using MassTransit;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Services.NotificationAPI;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

        foreach (var emailClient in builder.Configuration.GetSection("Emails").Get<EmailClient[]>())
        {
            builder.Services.AddSingleton<IEmailClient, EmailClient>(_ => emailClient);
        }

        builder.Services.AddDbContextFactory<NotificationDbContext>(options =>
        {
            options.UseSqlServer(connectionString,
                config =>
                {
                    config.EnableRetryOnFailure(3);
                    config.CommandTimeout(3);
                    config.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });

            options
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });

        builder.Services.AddLocalizer();
        builder.Services.AddScoped<EnqueueEmailConsumer>();
        builder.Services.AddScoped<EnqueuePushConsumer>();
        builder.Services.AddScoped<EnqueueSmsConsumer>();

        builder.Services.AddMassTransit(cfg =>
        {
            cfg.ConfigureBus(config =>
            {
                var assemblies = new[] { typeof(Program).GetType().Assembly };
                config.BusConfig = configurator => configurator.AddConsumers(assemblies);
            });
        });

        builder.Services.AddScoped<IHandler, EmailHandler>();
        builder.Services.AddScoped<IHandler, PushHandler>();
        builder.Services.AddScoped<IHandler, SmsHandler>();

        builder.Services.AddScoped<ITemplateService, TemplateService>();
        builder.Services.AddScoped<ICoordinator, Coordinator>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseRouting();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        await app.RunAsync();
    }
}