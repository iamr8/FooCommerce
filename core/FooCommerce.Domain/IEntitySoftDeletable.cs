namespace FooCommerce.Domain;

public interface IEntitySoftDeletable
{
    bool IsDeleted { get; set; }
}