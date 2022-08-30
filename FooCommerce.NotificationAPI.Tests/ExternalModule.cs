using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Core.JsonCustomization;
using FooCommerce.NotificationAPI.Consumers;

using MassTransit;

using Microsoft.Extensions.DependencyInjection;

using Module = Autofac.Module;

namespace FooCommerce.NotificationAPI.Tests;

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

    protected override void Load(ContainerBuilder builder)
    {
        var services = new ServiceCollection();
        services.AddMassTransitTestHarness(Apply);
        builder.Populate(services);

        Configuration.RegisterServices(builder);
    }
}