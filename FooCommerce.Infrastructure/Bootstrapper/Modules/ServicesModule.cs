using Autofac;

using FooCommerce.Infrastructure.Services;

namespace FooCommerce.Infrastructure.Bootstrapper.Modules;

public class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AccountService>()
            .As<IAccountService>()
            .InstancePerLifetimeScope();
    }
}