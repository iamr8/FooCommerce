using FooCommerce.NotificationAPI.Worker.Extensions;

using MassTransit;

using Microsoft.Extensions.DependencyInjection;

using Module = FooCommerce.Common.Configurations.Module;

namespace FooCommerce.NotificationAPI.Worker.Tests.Setup;

public class TestBusModule : Module
{
    public void Load(IServiceCollection services)
    {
        services.AddMassTransitTestHarness(cfg => cfg.ConfigureBus());
    }
}