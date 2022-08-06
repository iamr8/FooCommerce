namespace FooCommerce.Domain.Entities;

public interface IEntityHideable
{
    bool IsHidden { get; init; }
}