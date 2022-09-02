using Autofac;
using FooCommerce.MembershipAPI.Worker.Consumers;
using FooCommerce.MembershipAPI.Worker.Services;
using FooCommerce.MembershipAPI.Worker.Services.Repositories;
using MassTransit;

namespace FooCommerce.MembershipAPI.Worker;

public class ModuleConfiguration
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

        builder.RegisterType<CommunicationService>()
            .As<ICommunicationService>()
            .InstancePerLifetimeScope();

        builder.RegisterType<TokenService>()
            .As<ITokenService>()
            .InstancePerLifetimeScope();
    }
}