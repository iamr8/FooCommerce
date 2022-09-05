using FooCommerce.Common.Configurations;
using FooCommerce.NotificationAPI.Worker.Services;
using FooCommerce.NotificationAPI.Worker.Services.Repositories;

namespace FooCommerce.NotificationAPI.Worker.Modules;

public class ServicesModule : Module
{
    public void Load(IServiceCollection services)
    {
        services.AddScoped<INotificationClientService, NotificationClientService>();
        services.AddScoped<INotificationTemplateService, NotificationTemplateService>();
    }
}