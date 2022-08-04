namespace FooCommerce.Application.DbProvider.Interfaces
{
    public interface IEntitySortable
    {
        int Order { get; set; }
    }
}