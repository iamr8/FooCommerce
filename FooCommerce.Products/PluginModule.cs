using Autofac;

using FooCommerce.Products.Ads.Services;

namespace FooCommerce.Products;

public class PluginModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        if (builder.Properties.ContainsKey(GetType().AssemblyQualifiedName))
            return;

        builder.Properties.Add(GetType().AssemblyQualifiedName, null);
        Console.WriteLine($"Registering {GetType().AssemblyQualifiedName}");

        builder.RegisterType<PostingService>()
            .AsImplementedInterfaces()
            .InstancePerDependency();

        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.GetName().Name.StartsWith($"{nameof(FooCommerce)}.{nameof(FooCommerce.Products)}."));
        builder.RegisterAssemblyModules(assemblies.ToArray());
        base.Load(builder);
    }
}