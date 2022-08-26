using System.Net.Mime;
using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Application.Notifications.Services;
using FooCommerce.Infrastructure.JsonCustomization;
using FooCommerce.NotificationAPI.Consumers;
using FooCommerce.NotificationAPI.Services;

using MassTransit;

using Microsoft.Extensions.DependencyInjection;

using Module = Autofac.Module;

namespace FooCommerce.NotificationAPI.Modules;

public class NotificationAPIModule : Module
{
    private readonly bool _test;

    public NotificationAPIModule(bool test)
    {
        _test = test;
    }

    private void Apply(IBusRegistrationConfigurator cfg)
    {
        var entryAssembly = typeof(QueueNotificationConsumer).Assembly;
        cfg.SetKebabCaseEndpointNameFormatter();
        cfg.SetInMemorySagaRepositoryProvider();

        cfg.AddMediator();

        cfg.AddConsumers(entryAssembly);
        //cfg.AddSagaStateMachines(entryAssembly);
        cfg.AddSagas(entryAssembly);
        cfg.AddActivities(entryAssembly);
        //cfg.AddRequestClient<QueueNotification>(new Uri($"queue:{KebabCaseEndpointNameFormatter.Instance.Consumer<QueueNotificationConsumer>()}"));

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

        builder.RegisterType<NotificationTemplateService>()
            .As<INotificationTemplateService>()
            .InstancePerLifetimeScope();
    }
}