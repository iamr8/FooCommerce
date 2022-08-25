using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Application.Services.Notifications;
using FooCommerce.Infrastructure.JsonCustomization;
using FooCommerce.NotificationAPI.Services;

using MassTransit;
using MassTransit.Serialization;

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
        NewtonsoftJsonMessageSerializer.SerializerSettings = DefaultSettings.Settings;
        NewtonsoftJsonMessageSerializer.DeserializerSettings = DefaultSettings.Settings;

        cfg.SetKebabCaseEndpointNameFormatter();
        cfg.SetInMemorySagaRepositoryProvider();

        cfg.AddConsumers(this.GetType().Assembly);
        cfg.AddSagaStateMachines(this.GetType().Assembly);

        if (!_test)
        {
            cfg.UsingGrpc((context, config) =>
            {
                config.ConfigureNewtonsoftJsonDeserializer(s => DefaultSettings.Settings);
                config.ConfigureNewtonsoftJsonSerializer(s => DefaultSettings.Settings);

                config.AutoStart = true;
                config.Host(h =>
                {
                    h.Host = "127.0.0.1";
                    h.Port = 19796;

                    h.AddServer(new Uri("http://127.0.0.1:19797"));
                    h.AddServer(new Uri("http://127.0.0.1:19798"));
                });

                config.ConfigureEndpoints(context);
            });
        }
        else
        {
            cfg.UsingInMemory((context, config) =>
            {
                config.ConfigureNewtonsoftJsonDeserializer(s => DefaultSettings.Settings);
                config.ConfigureNewtonsoftJsonSerializer(s => DefaultSettings.Settings);

                config.AutoStart = true;
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