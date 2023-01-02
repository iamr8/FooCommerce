namespace FooCommerce.Domain.Pagination;

public interface IPage
{
    /// <summary>
    /// Represents an <see cref="int"/> value that representing Current Page Number
    /// </summary>
    int PageNo { get; set; }
}