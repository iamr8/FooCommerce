using Autofac;

using FooCommerce.Application.Helpers;

namespace FooCommerce.Infrastructure.Modules;

public class NotificationAPIModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = AppDomain.CurrentDomain
            .GetExecutingAssemblies()
            .Single(x => x.GetName().Name.Equals("FooCommerce.NotificationAPI"));

        builder.RegisterAssemblyModules(assembly);
    }
}