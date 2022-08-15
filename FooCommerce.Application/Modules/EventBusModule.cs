using Autofac;
using Autofac.Extensions.DependencyInjection;

using MassTransit;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Application.Modules
{
    public class EventBusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();
            services.AddMassTransit(configurator =>
            {
                configurator.UsingInMemory((context, cfg) =>
                {
                    cfg.AutoStart = true;
                    cfg.ConfigureEndpoints(context);
                });
            });

            builder.Populate(services);
        }
    }
}