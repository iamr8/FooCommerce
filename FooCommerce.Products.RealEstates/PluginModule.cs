using Autofac;

using FooCommerce.Domain.Plugins.Interfaces;
using FooCommerce.Products.Domain.Models;
using FooCommerce.Products.RealEstates.Application.Models;
using FooCommerce.Products.RealEstates.Commands;
using FooCommerce.Products.RealEstates.Entities;

using MediatR;

namespace FooCommerce.Products.RealEstates;

public class PluginModule : Module, IPlugin
{
    public string Name => nameof(RealEstate);
    public bool Active => true;
    public string[] Tags => new[] { "realestate", "land" };

    protected override void Load(ContainerBuilder builder)
    {
        if (builder.Properties.ContainsKey(GetType().AssemblyQualifiedName))
            return;

        builder.Properties.Add(GetType().AssemblyQualifiedName, null);
        Console.WriteLine($"Registering {GetType().AssemblyQualifiedName}");

        builder.RegisterType<NewRealEstateAdHandler>()
            .AsImplementedInterfaces()
            .InstancePerDependency();
        base.Load(builder);
    }
}