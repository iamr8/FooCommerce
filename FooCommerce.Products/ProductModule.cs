using Autofac;

using FooCommerce.Application;

namespace FooCommerce.Products;

public class ProductModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        if (builder.Properties.ContainsKey(GetType().AssemblyQualifiedName))
            return;

        builder.Properties.Add(GetType().AssemblyQualifiedName, null);
        Console.WriteLine($"Registering {GetType().AssemblyQualifiedName}");

        var pluginsAssemblies = AppDomain.CurrentDomain.GetExecutingAssemblies().Where(x => x.GetName().Name.StartsWith($"{nameof(FooCommerce)}.{nameof(FooCommerce.Products)}."));
        builder.RegisterAssemblyModules(pluginsAssemblies.ToArray());

        base.Load(builder);
    }
}