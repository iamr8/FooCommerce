namespace FooCommerce.Domain.Plugins.Interfaces;

public interface IPlugin
{
    string Name { get; }
    bool Active { get; }
    string[] Tags { get; }
}