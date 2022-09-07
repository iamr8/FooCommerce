using FooCommerce.Common.Helpers;
using FooCommerce.EventSource;

using MassTransit;

using Microsoft.Extensions.DependencyInjection;

using Module = FooCommerce.Common.Configurations.Module;

namespace FooCommerce.NotificationAPI.Worker.Tests.Setup;

public class TestBusModule : Module
{
    public void Load(IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetExecutingAssemblies().ToArray();
        services.AddMassTransitTestHarness(cfg => cfg.ConfigureBus(assemblies));
    }
}