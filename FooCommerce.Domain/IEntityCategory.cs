namespace FooCommerce.Domain;

public interface IEntityCategory
{
    string Name { get; init; }
    string Description { get; init; }
    string Icon { get; init; }
}