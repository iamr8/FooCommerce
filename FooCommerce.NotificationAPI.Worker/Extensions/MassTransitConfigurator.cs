using FooCommerce.Common.Configurations;

using MassTransit;

namespace FooCommerce.NotificationAPI.Worker.Extensions;

public static class MassTransitConfigurator
{
    public static void ConfigureBus(this IBusRegistrationConfigurator cfg)
    {
        cfg.SetKebabCaseEndpointNameFormatter();
        // cfg.SetInMemorySagaRepositoryProvider();
        cfg.AddMediator();

        cfg.AddConsumers(typeof(Worker).Assembly);

        cfg.UsingInMemory((context, config) =>
        {
            config.UseRawJsonDeserializer();
            config.UseRawJsonSerializer();

            config.AutoStart = true;

            //if (ContainerSettings.IsRunningInContainer)
            //{
            //    config.Host(RabbitMQConfiguration.Host);
            //}
            //else
            //{
            //    config.Host(RabbitMQConfiguration.Host, RabbitMQConfiguration.VirtualHost, h =>
            //    {
            //        h.Username(RabbitMQConfiguration.Username);
            //        h.Password(RabbitMQConfiguration.Password);
            //    });
            //}

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
    }
}