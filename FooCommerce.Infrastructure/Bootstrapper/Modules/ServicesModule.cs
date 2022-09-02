using Autofac;

using FooCommerce.Infrastructure.Services;
using FooCommerce.Infrastructure.Services.Repositories;

namespace FooCommerce.Infrastructure.Bootstrapper.Modules;

public class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AccountService>()
            .As<IAccountService>()
            .InstancePerLifetimeScope();

        builder.RegisterType<NotificationService>()
            .As<INotificationService>()
            .InstancePerLifetimeScope();

        builder.RegisterType<LocationService>()
            .As<ILocationService>()
            .InstancePerLifetimeScope();
    }
}