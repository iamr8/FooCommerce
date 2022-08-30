using Autofac;

using FooCommerce.Application.Notifications.Services;
using FooCommerce.Core.Modules;
using FooCommerce.NotificationAPI.Consumers;
using FooCommerce.NotificationAPI.Services;

using MassTransit;

namespace FooCommerce.NotificationAPI;

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