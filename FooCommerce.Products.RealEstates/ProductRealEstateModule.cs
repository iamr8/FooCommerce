using Autofac;

using FooCommerce.Products.RealEstates.Commands;

using MassTransit;

namespace FooCommerce.Products.RealEstates;

public class ProductRealEstateModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        if (builder.Properties.ContainsKey(GetType().AssemblyQualifiedName))
            return;

        builder.Properties.Add(GetType().AssemblyQualifiedName, null);
        Console.WriteLine($"Registering {GetType().AssemblyQualifiedName}");

        builder.Register(context => Bus.Factory.CreateUsingInMemory(cfg =>
        {
            cfg.ReceiveEndpoint("create-realestate-ad", endpoint_cfg =>
            {
                endpoint_cfg.Instance(new CreateRealEstateAdConsumer());
            });
        }));

        base.Load(builder);
    }
}