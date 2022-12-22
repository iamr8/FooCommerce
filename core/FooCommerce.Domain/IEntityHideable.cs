namespace FooCommerce.Domain;

public interface IEntityHideable
{
    bool IsHidden { get; init; }
}