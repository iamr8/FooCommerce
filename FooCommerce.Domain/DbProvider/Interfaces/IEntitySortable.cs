namespace FooCommerce.Domain.DbProvider.Interfaces
{
    public interface IEntitySortable
    {
        int Order { get; set; }
    }
}