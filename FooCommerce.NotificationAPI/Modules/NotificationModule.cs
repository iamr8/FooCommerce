using Autofac;

using FooCommerce.Application.Services.Notifications;
using FooCommerce.NotificationAPI.Services;

namespace FooCommerce.NotificationAPI.Modules;

public class NotificationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<NotificationTemplateService>()
            .As<INotificationTemplateService>()
            .InstancePerLifetimeScope();
    }
}