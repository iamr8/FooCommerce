using Autofac;
using FooCommerce.Core.Modules;
using FooCommerce.NotificationAPI.Services;
using FooCommerce.NotificationAPI.Worker.Consumers;
using FooCommerce.NotificationAPI.Worker.Services;
using MassTransit;

namespace FooCommerce.NotificationAPI.Worker;

public class ModuleConfiguration : IModuleConfiguration
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