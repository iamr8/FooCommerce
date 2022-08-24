using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Application.Helpers;

using MassTransit;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Modules;

public class EventBusModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var services = new ServiceCollection();

        var assemblies = AppDomain.CurrentDomain.GetExecutingAssemblies().ToArray();
        services.AddMediatR(assemblies);

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