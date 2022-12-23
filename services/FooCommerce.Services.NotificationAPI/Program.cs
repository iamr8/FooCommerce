using FooCommerce.Domain.Jsons;
using FooCommerce.EventSource;
using FooCommerce.Localization.DependencyInjection;
using FooCommerce.NotificationService.Consumers;
using FooCommerce.NotificationService.Contracts;
using FooCommerce.NotificationService.DbProvider;
using FooCommerce.NotificationService.Handlers;
using FooCommerce.NotificationService.Interfaces;
using FooCommerce.NotificationService.Models;
using FooCommerce.NotificationService.Services;
using FooCommerce.NotificationService.Services.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace FooCommerce.NotificationService;

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

            if (builder.Environment.IsDevelopment())
            {
                options
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        });

        builder.Services
            .AddMvc()
            .AddJsonOptions(options =>
            {
                foreach (var jsonConverter in JsonDefaultSettings.Settings.Converters)
                {
                    var duplicateConverter = options.JsonSerializerOptions.Converters.Any(x => x.GetType() == jsonConverter.GetType());
                    if (!duplicateConverter)
                        options.JsonSerializerOptions.Converters.Add(jsonConverter);
                }

                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonDefaultSettings.Settings.DefaultIgnoreCondition;
                options.JsonSerializerOptions.UnknownTypeHandling = JsonDefaultSettings.Settings.UnknownTypeHandling;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonDefaultSettings.Settings.DictionaryKeyPolicy;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = JsonDefaultSettings.Settings.PropertyNameCaseInsensitive;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonDefaultSettings.Settings.PropertyNamingPolicy;
                options.JsonSerializerOptions.WriteIndented = true;
            });
        builder.Services.AddControllers();

        builder.Services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = false;
        });

        builder.Services.AddLocalizer();
        builder.Services.AddScoped<EnqueueEmailConsumer>();
        builder.Services.AddScoped<EnqueuePushConsumer>();
        builder.Services.AddScoped<EnqueueSmsConsumer>();

        builder.Services.AddMassTransit(cfg =>
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