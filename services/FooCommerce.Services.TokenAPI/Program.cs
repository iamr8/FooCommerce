using System.Reflection;
using FooCommerce.Domain.Jsons;
using FooCommerce.EventSource;
using FooCommerce.TokenService.Sagas;
using MassTransit;

namespace FooCommerce.TokenService;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
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

        builder.Services.AddMassTransit(cfg =>
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