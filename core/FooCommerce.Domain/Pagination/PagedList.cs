using System.Collections;
using System.Text;
using System.Text.Json.Serialization;

namespace FooCommerce.Domain.Pagination;

/// <summary>
/// Initializes an <see cref="PagedList{TModel}"/> instance.
/// </summary>
[Serializable]
public struct PagedList<TModel> : IPagedList, IPagedListFiltered, IReadOnlyList<TModel> where TModel : class
{
    /// <summary>
    /// Initializes an <see cref="PagedList{TModel}"/> instance.
    /// </summary>
    /// <param name="pageNo">An <see cref="int"/> value that representing page number that contains these data</param>
    /// <param name="pages">An <see cref="int"/> value that representing page numbers</param>
    /// <param name="totalCount">An <see cref="int"/> value that representing loaded data count</param>
    public PagedList(int pageNo, int pages, int totalCount) : this()
    {
        Pages = pages < 0 ? 0 : pages;
        PageNo = Pages > 0
            ? pageNo <= 0 ? 1 : pageNo
            : 0;
        TotalCount = totalCount < 0 ? 0 : totalCount;
    }

    [JsonPropertyName("pageNo")]
    public int PageNo { get; set; } = 1;

    [JsonPropertyName("pages")]
    public int Pages { get; set; } = 1;

    [JsonPropertyName("total")]
    public int TotalCount { get; set; } = 0;

    [JsonIgnore]
    public bool HasNextPage => PageNo < Pages;

    [JsonIgnore]
    public bool HasPreviousPage => PageNo > 1;

    /// <summary>
    /// Initializes an <see cref="PagedList{TModel}"/> instance.
    /// </summary>
    /// <param name="page">An <see cref="int"/> value that representing page number that contains these data</param>
    /// <param name="pages">An <see cref="int"/> value that representing page numbers</param>
    /// <param name="totalCount">An <see cref="int"/> value that representing loaded data count</param>
    public PagedList(int page, int pages, int totalCount, IPagedListFilter filter) : this(page, pages, totalCount)
    {
        Filter = filter;
    }

    /// <summary>
    /// Initializes an <see cref="PagedList{TModel}"/> instance.
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="filter"></param>
    public PagedList(IPagedList instance, IPagedListFilter filter) : this(instance.PageNo, instance.Pages, instance.TotalCount, filter)
    {
    }

    /// <summary>
    /// Initializes an <see cref="PagedList{TModel}"/> instance.
    /// </summary>
    /// <param name="instance"></param>
    public PagedList(IPagedList instance) : this(instance.PageNo, instance.Pages, instance.TotalCount)
    {
        if (instance is IPagedListFiltered filtered)
            this.Filter = filtered.Filter;
    }

    /// <summary>
    /// Initializes an <see cref="PagedList{TModel}"/> instance.
    /// </summary>
    /// <param name="items">n <see cref="List{T}"/> that representing values should be loaded into pagination</param>
    /// <param name="page">An <see cref="int"/> value that representing page number that contains these data</param>
    /// <param name="pages">An <see cref="int"/> value that representing page numbers</param>
    /// <param name="totalCount">An <see cref="int"/> value that representing loaded data count</param>
    public PagedList(IEnumerable<TModel> items, int page, int pages, int totalCount = 0) : this(page, pages, totalCount, PagedListFilter.Empty)
    {
        Items = items?.Any() == true ? items.ToList() : new List<TModel>();
    }

    /// <summary>
    /// An <see cref="List{T}"/> that representing values being loaded into pagination
    /// </summary>
    [JsonPropertyName("results")]
    public IReadOnlyList<TModel> Items { get; set; } = new List<TModel>();

    [JsonIgnore]
    public IPagedListFilter Filter { get; set; }

    public IEnumerator<TModel> GetEnumerator()
    {
        return Items?.GetEnumerator() ?? Enumerable.Empty<TModel>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerable<TModel> this[Range range] => this.Items.ToArray()[range];

    public int Count => this.Items?.Count ?? 0;

    public TModel this[int index] => this.Items[index];

    public static PagedList<TModel> Empty = new();

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("Page ");
        sb.Append(PageNo);
        sb.Append('/');
        sb.Append(Pages);
        return sb.ToString();
    }
}