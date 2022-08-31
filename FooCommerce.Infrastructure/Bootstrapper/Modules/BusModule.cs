using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Application.Configurations;
using FooCommerce.Core;

using MassTransit;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Bootstrapper.Modules;

public class BusModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var services = new ServiceCollection();
        services.AddMassTransit(cfg =>
        {
            var entryAssembly = GetType().Assembly;
            cfg.SetKebabCaseEndpointNameFormatter();
            cfg.SetInMemorySagaRepositoryProvider();
            cfg.AddMediator();

            cfg.AddConsumers(GetType().Assembly);

            cfg.UsingRabbitMq((context, config) =>
            {
                config.Host(RabbitMQConfiguration.Host, RabbitMQConfiguration.VirtualHost, h =>
                {
                    h.Username(RabbitMQConfiguration.Username);
                    h.Password(RabbitMQConfiguration.Password);
                });

                config.ConfigureJsonSerializerOptions(options =>
                {
                    options.Converters.Clear();
                    foreach (var jsonConverter in JsonDefaultSettings.Settings.Converters)
                        options.Converters.Add(jsonConverter);

                    options.DefaultIgnoreCondition = JsonDefaultSettings.Settings.DefaultIgnoreCondition;
                    options.UnknownTypeHandling = JsonDefaultSettings.Settings.UnknownTypeHandling;
                    options.DictionaryKeyPolicy = JsonDefaultSettings.Settings.DictionaryKeyPolicy;
                    options.PropertyNameCaseInsensitive = JsonDefaultSettings.Settings.PropertyNameCaseInsensitive;
                    options.PropertyNamingPolicy = JsonDefaultSettings.Settings.PropertyNamingPolicy;
                    options.WriteIndented = true;
                    return options;
                });

                config.ConfigureEndpoints(context);
            });
        });
        builder.Populate(services);
    }
}