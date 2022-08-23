namespace FooCommerce.Domain.Interfaces.Database
{
    public interface IEntitySortable
    {
        int Order { get; set; }
    }
}