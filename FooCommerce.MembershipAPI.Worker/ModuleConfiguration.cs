using Autofac;

using FooCommerce.Core.Modules;

using MassTransit;

namespace FooCommerce.MembershipAPI.Worker;

public class ModuleConfiguration : IModuleConfiguration
{
    public void AddConsumers(IBusRegistrationConfigurator cfg)
    {
    }

    public void RegisterServices(ContainerBuilder builder)
    {
    }
}