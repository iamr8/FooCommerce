namespace FooCommerce.Domain.Interfaces.Database;

public interface IEntityHideable
{
    bool IsHidden { get; init; }
}