using FooCommerce.Common.Configurations;

using MassTransit;

namespace FooCommerce.EventSource;

public static class MassTransitBrokerConfigurator
{
    private static void ConfigureBusFactory(this IBusFactoryConfigurator config)
    {
        config.UseRawJsonDeserializer();
        config.UseRawJsonSerializer();

        config.AutoStart = true;
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
    }

    public static void UsingPreferredTransport(this IBusRegistrationConfigurator cfg, Action<IBusRegistrationContext> customConfig = null)
    {
        cfg.UsingInMemory((context, config) =>
        {
            config.ConfigureBusFactory();
            customConfig?.Invoke(context);
            config.ConfigureEndpoints(context);
        });
    }
}