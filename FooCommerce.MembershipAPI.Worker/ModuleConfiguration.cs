using Autofac;

using FooCommerce.Core.Modules;
using FooCommerce.MembershipAPI.Services;
using FooCommerce.MembershipAPI.Worker.Consumers;
using FooCommerce.MembershipAPI.Worker.Services;
using MassTransit;

namespace FooCommerce.MembershipAPI.Worker;

public class ModuleConfiguration : IModuleConfiguration
{
    public void AddConsumers(IBusRegistrationConfigurator cfg)
    {
        var entryAssembly = typeof(UpdateAuthTokenStateConsumer).Assembly;
        cfg.AddConsumers(entryAssembly);
    }

    public void RegisterServices(ContainerBuilder builder)
    {
        builder.RegisterType<UserService>()
            .As<IUserService>()
            .InstancePerLifetimeScope();

        builder.RegisterType<VerificationService>()
            .As<IVerificationService>()
            .InstancePerLifetimeScope();
    }
}