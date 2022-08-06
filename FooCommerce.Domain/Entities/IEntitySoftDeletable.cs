namespace FooCommerce.Domain.Entities;

public interface IEntitySoftDeletable
{
    bool IsDeleted { get; init; }
}