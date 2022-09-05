using FooCommerce.Common.Configurations;
using FooCommerce.Infrastructure.Services;
using FooCommerce.Infrastructure.Services.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Bootstrapper.Modules;

public class ServicesModule : Module
{
    public void Load(IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IAccountService, AccountService>();
    }
}