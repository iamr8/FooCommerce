namespace FooCommerce.Domain.Pagination;

public interface IPagedListFiltered
{
    IPagedListFilter Filter { get; }
}