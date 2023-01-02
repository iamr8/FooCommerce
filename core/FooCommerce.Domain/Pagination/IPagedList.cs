namespace FooCommerce.Domain.Pagination;

public interface IPagedList : IPage
{
    /// <summary>
    /// An <see cref="int"/> value that representing page numbers
    /// </summary>
    int Pages { get; }

    /// <summary>
    /// An <see cref="int"/> value that representing loaded data count
    /// </summary>
    int TotalCount { get; }

    bool HasNextPage { get; }

    bool HasPreviousPage { get; }
}