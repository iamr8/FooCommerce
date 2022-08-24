using System.Reflection;

using Autofac;

using MassTransit;
using MassTransit.AutofacIntegration;

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