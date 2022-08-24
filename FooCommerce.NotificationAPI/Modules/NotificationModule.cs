using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Application.Services.Notifications;
using FooCommerce.NotificationAPI.Services;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.NotificationAPI.Modules;

public class NotificationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var services = new ServiceCollection();

        services.AddScoped<INotificationTemplateService, NotificationTemplateService>();

        builder.Populate(services);
    }
}