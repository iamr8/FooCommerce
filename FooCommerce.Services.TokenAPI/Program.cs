using FooCommerce.EventSource;
using FooCommerce.Services.TokenAPI.Sagas;

using MassTransit;

namespace FooCommerce.Services.TokenAPI;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
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
        builder.Services.AddControllers();

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