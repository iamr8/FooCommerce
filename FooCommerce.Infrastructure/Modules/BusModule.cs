using System.Reflection;

using Autofac;
using Autofac.Core;

using FooCommerce.Infrastructure.Shopping.Contracts;

using MassTransit;
using MassTransit.AutofacIntegration;
using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Modules;

public class BusModule : Autofac.Module
{
    private readonly bool _test;
    private readonly IEnumerable<Assembly> _assemblies;

    public BusModule(params Assembly[] assemblies)
    {
        _assemblies = assemblies;
    }

    public BusModule(bool test, params Assembly[] assemblies) : this(assemblies)
    {
        _test = test;
        _assemblies = assemblies;
    }

    private static void ApplyConfigurator(IContainerBuilderBusConfigurator cfg, params Assembly[] assemblies)
    {
        cfg.SetKebabCaseEndpointNameFormatter();
        cfg.SetInMemorySagaRepositoryProvider();

        cfg.AddConsumers(assemblies);
        cfg.AddSagaStateMachines(assemblies);
        cfg.AddSagas(assemblies);
        cfg.AddActivities(assemblies);

        cfg.UsingGrpc((context, cfg) =>
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
    }

    protected override void Load(ContainerBuilder builder)
    {
        if (_test)
        {
            builder.AddMassTransitInMemoryTestHarness(cfg => ApplyConfigurator(cfg, _assemblies.ToArray()));
        }
        else
        {
            builder.AddMassTransit(cfg => ApplyConfigurator(cfg, _assemblies.ToArray()));
        }
    }
}