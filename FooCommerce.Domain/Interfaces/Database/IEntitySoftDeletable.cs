namespace FooCommerce.Domain.Interfaces.Database;

public interface IEntitySoftDeletable
{
    bool IsDeleted { get; init; }
}