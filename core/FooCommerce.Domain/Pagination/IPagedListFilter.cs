namespace FooCommerce.Domain.Pagination;

/// <summary>
/// An <see cref="IPagedListFilter"/> interface
/// </summary>
public interface IPagedListFilter : IPage
{
    /// <summary>
    /// Represents an <see cref="int"/> value that representing Count of Items in Current Page
    /// </summary>
    int PageSize { get; set; }

    IDictionary<string, string> ToDictionary(bool observeEndpoint = false);
}