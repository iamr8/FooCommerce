namespace FooCommerce.Domain.Entities
{
    public interface IEntitySortable
    {
        int Order { get; set; }
    }
}