using Autofac;

using MassTransit;

namespace FooCommerce.Products.PurchasableItems;

public class ProductPurchasableItemsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        if (builder.Properties.ContainsKey(GetType().AssemblyQualifiedName))
            return;

        builder.Properties.Add(GetType().AssemblyQualifiedName, null);
        Console.WriteLine($"Registering {GetType().AssemblyQualifiedName}");

        builder.Register(context => Bus.Factory.CreateUsingInMemory(cfg =>
        {
            //cfg.ReceiveEndpoint("create-realestate-ad", endpoint_cfg =>
            //{
            //    endpoint_cfg.Instance(new CreatePurchasableItemConsumer());
            //});
        }));

        base.Load(builder);
    }
}