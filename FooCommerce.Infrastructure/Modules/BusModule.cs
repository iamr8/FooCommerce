using System.Reflection;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using MassTransit;

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

    private static void ApplyConfigurator(IRegistrationConfigurator cfg, params Assembly[] assemblies)
    {
        cfg.SetKebabCaseEndpointNameFormatter();
        cfg.SetInMemorySagaRepositoryProvider();

        cfg.AddConsumers(assemblies);
        cfg.AddSagaStateMachines(assemblies);
        cfg.AddSagas(assemblies);
        cfg.AddActivities(assemblies);
    }

    protected override void Load(ContainerBuilder builder)
    {
        var serviceCollection = new ServiceCollection();

        if (_test)
        {
            serviceCollection.AddMassTransitTestHarness(cfg => ApplyConfigurator(cfg, _assemblies.ToArray()));
        }
        else
        {
            serviceCollection.AddMassTransit(cfg => ApplyConfigurator(cfg, _assemblies.ToArray()));
        }

        builder.Populate(serviceCollection);
        base.Load(builder);
    }
}