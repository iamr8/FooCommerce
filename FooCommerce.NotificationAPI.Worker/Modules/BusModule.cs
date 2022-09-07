using FooCommerce.Common.Configurations;
using FooCommerce.EventSource;

using MassTransit;

namespace FooCommerce.NotificationAPI.Worker.Modules;

public class BusModule : Module
{
    public void Load(IServiceCollection services)
    {
        services.AddMassTransit(cfg => cfg.ConfigureBus(this.GetType().Assembly));
    }
}