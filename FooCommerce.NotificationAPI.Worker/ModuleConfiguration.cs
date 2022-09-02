using Autofac;

using FooCommerce.NotificationAPI.Worker.Consumers;
using FooCommerce.NotificationAPI.Worker.Services;
using FooCommerce.NotificationAPI.Worker.Services.Repositories;

using MassTransit;

namespace FooCommerce.NotificationAPI.Worker;

public class ModuleConfiguration
{
    public void AddConsumers(IBusRegistrationConfigurator cfg)
    {
        var entryAssembly = typeof(QueueNotificationConsumer).Assembly;
        cfg.AddConsumers(entryAssembly);
    }

    public void RegisterServices(ContainerBuilder builder)
    {
        builder.RegisterType<NotificationClientService>()
            .As<INotificationClientService>()
            .InstancePerLifetimeScope();

        builder.RegisterType<NotificationTemplateService>()
            .As<INotificationTemplateService>()
            .InstancePerLifetimeScope();
    }
}