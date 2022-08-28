using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Infrastructure.JsonCustomization;

using MassTransit;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Modules;

public class BusModule : Autofac.Module
{
    private readonly bool _test;

    public BusModule(bool test = false)
    {
        _test = test;
    }

    private void Apply(IBusRegistrationConfigurator cfg)
    {
        var entryAssembly = this.GetType().Assembly;
        cfg.SetKebabCaseEndpointNameFormatter();
        cfg.SetInMemorySagaRepositoryProvider();

        cfg.AddMediator();

        cfg.AddConsumers(entryAssembly);
        cfg.AddSagas(entryAssembly);
        cfg.AddActivities(entryAssembly);

        if (!_test)
        {
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
        }
        else
        {
            cfg.UsingInMemory((context, config) =>
            {
                config.UseRawJsonDeserializer();
                config.UseRawJsonSerializer();

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
                    options.ReferenceHandler = DefaultSettings.Settings.ReferenceHandler;
                    options.WriteIndented = true;
                    return options;
                });
                config.ConfigureEndpoints(context);
            });
        }
    }

    protected override void Load(ContainerBuilder builder)
    {
        var services = new ServiceCollection();

        if (_test)
        {
            services.AddMassTransitTestHarness(Apply);
        }
        else
        {
            services.AddMassTransit(Apply);
        }
        builder.Populate(services);
    }
}