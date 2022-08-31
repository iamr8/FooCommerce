using Autofac;
using Autofac.Extensions.DependencyInjection;
using FooCommerce.Core;
using FooCommerce.NotificationAPI.Worker.Consumers;

using MassTransit;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.NotificationAPI.Worker.Tests;

public class ExternalModule : Module
{
    private readonly bool _test;
    public ModuleConfiguration Configuration { get; }

    public ExternalModule(bool test)
    {
        _test = test;
        Configuration = new ModuleConfiguration();
    }

    private void Apply(IBusRegistrationConfigurator cfg)
    {
        var entryAssembly = typeof(QueueNotificationConsumer).Assembly;
        cfg.SetKebabCaseEndpointNameFormatter();
        cfg.SetInMemorySagaRepositoryProvider();

        cfg.AddMediator();

        Configuration.AddConsumers(cfg);
        //cfg.AddSagaStateMachines(entryAssembly);
        //cfg.AddSagas(entryAssembly);
        //cfg.AddActivities(entryAssembly);
        //cfg.AddRequestClient<QueueNotification>(new Uri($"queue:{KebabCaseEndpointNameFormatter.Instance.Consumer<QueueNotificationConsumer>()}"));

        cfg.UsingInMemory((context, config) =>
        {
            config.UseRawJsonDeserializer();
            config.UseRawJsonSerializer();

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
                options.ReferenceHandler = JsonDefaultSettings.Settings.ReferenceHandler;
                options.WriteIndented = true;
                return options;
            });
            config.ConfigureEndpoints(context);
        });
    }

    protected override void Load(ContainerBuilder builder)
    {
        var services = new ServiceCollection();
        services.AddMassTransitTestHarness(Apply);
        builder.Populate(services);

        Configuration.RegisterServices(builder);
    }
}