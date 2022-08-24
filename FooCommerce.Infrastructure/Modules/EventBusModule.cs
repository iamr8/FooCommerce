using Autofac;

using MassTransit;

namespace FooCommerce.Infrastructure.Modules;

public class EventBusModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddMassTransit(configurator =>
        {
            configurator.UsingGrpc((context, cfg) =>
            {
                cfg.AutoStart = true;
                cfg.Host(h =>
                {
                    h.Host = "127.0.0.1";
                    h.Port = 19796;

                    h.AddServer(new Uri("http://127.0.0.1:19797"));
                    h.AddServer(new Uri("http://127.0.0.1:19798"));
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}