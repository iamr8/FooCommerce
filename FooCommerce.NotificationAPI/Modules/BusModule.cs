using Autofac;

using FooCommerce.Core.JsonCustomization;
using FooCommerce.NotificationAPI;

using MassTransit;

namespace FooCommerce.NotificationAPI.Modules;

public class BusModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var moduleConfiguration = new ModuleConfiguration();

        builder.AddMassTransit(cfg =>
        {
            var entryAssembly = GetType().Assembly;
            cfg.SetKebabCaseEndpointNameFormatter();
            cfg.SetInMemorySagaRepositoryProvider();
            cfg.AddMediator();

            moduleConfiguration.AddConsumers(cfg);

            cfg.UsingRabbitMq((context, config) =>
            {
                config.AutoStart = true;
                config.Host("localhost", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                config.ConfigureJsonSerializerOptions(options =>
                {
                    options.Converters.Clear();
                    foreach (var jsonConverter in DefaultSettings.Settings.Converters)
                        options.Converters.Add(jsonConverter);

                    options.DefaultIgnoreCondition = DefaultSettings.Settings.DefaultIgnoreCondition;
                    options.UnknownTypeHandling = DefaultSettings.Settings.UnknownTypeHandling;
                    options.DictionaryKeyPolicy = DefaultSettings.Settings.DictionaryKeyPolicy;
                    options.PropertyNameCaseInsensitive = DefaultSettings.Settings.PropertyNameCaseInsensitive;
                    options.PropertyNamingPolicy = DefaultSettings.Settings.PropertyNamingPolicy;
                    options.WriteIndented = true;
                    return options;
                });

                config.ConfigureEndpoints(context);
            });
        });

        moduleConfiguration.RegisterServices(builder);
    }
}