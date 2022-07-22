using FooCommerce.Domain.Plugins.Interfaces;
using FooCommerce.Products.Vehicles.Entities;

namespace FooCommerce.Products.Vehicles;

public class PluginRuntime : IPlugin
{
    public string Name => nameof(Vehicle);
    public bool Active => true;
    public string[] Tags => new[] { "vehicle", "car", "ship", "boat", "bike" };
}