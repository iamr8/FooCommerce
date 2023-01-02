namespace FooCommerce.Domain.Pagination;

public abstract record PagedListFilter : IPagedListFilter
{
    public virtual int PageNo { get; set; }
    public virtual int PageSize { get; set; }

    public virtual IDictionary<string, string> ToDictionary(bool observeEndpoint = false) =>
        throw new NotImplementedException();

    public static EmptyPagedListFilter Empty => new();

    public record EmptyPagedListFilter : PagedListFilter
    {
    }
}