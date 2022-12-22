using FooCommerce.Common.Configurations;
using FooCommerce.IdentityAPI.Worker.Consumers;

namespace FooCommerce.IdentityAPI.Worker.Modules;

public class BusModule : Module
{
    public void Load(IServiceCollection services)
    {
        services.AddScoped<CreateUserConsumer>();
        services.AddScoped<GetUserClaimsConsumer>();

        // services.AddScoped<TokenGeneratedConsumer>();
        // services.AddScoped<TokenValidatedConsumer>();
        // services.AddScoped<TokenNotValidatedConsumer>();
        // services.AddScoped<TokenInvalidatedConsumer>();
        // services.AddScoped<TokenExpiredConsumer>();

        //services.AddMassTransit(cfg =>
        //{
        //    cfg.ConfigureBus(config =>
        //    {
        //        config.BusConfig = configurator =>
        //        {
        //            configurator.AddSagaStateMachine<TokenStateMachine, TokenState>().InMemoryRepository();
        //        };
        //    });
        //});
    }
}