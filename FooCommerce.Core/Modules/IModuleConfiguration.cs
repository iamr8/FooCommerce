using Autofac;

using MassTransit;

namespace FooCommerce.Core.Modules;

public interface IModuleConfiguration
{
    void AddConsumers(IBusRegistrationConfigurator cfg);

    void RegisterServices(ContainerBuilder builder);
}