namespace FooCommerce.Domain.Models.Paginator
{
    /// <summary>
    /// Initializes an <see cref="Pagination{TResult}"/> instance.
    /// </summary>
    [Serializable]
    public struct Pagination<TModel> : IPagination where TModel : class
    {
        /// <summary>
        /// Initializes an <see cref="Pagination{TResult}"/> instance.
        /// </summary>
        /// <param name="items">n <see cref="List{T}"/> that representing values should be loaded into pagination</param>
        /// <param name="currentPage">An <see cref="int"/> value that representing page number that contains these data</param>
        /// <param name="pages">An <see cref="int"/> value that representing page numbers</param>
        /// <param name="countAll">An <see cref="int"/> value that representing loaded data count</param>
        public Pagination(IEnumerable<TModel> items, int currentPage, int pages, int countAll = 0)
        {
            Pages = pages < 0 ? 0 : pages;
            Page = Pages > 0
                ? currentPage <= 0 ? 1 : currentPage
                : 0;
            Count = countAll < 0 ? 0 : countAll;
            Items = items?.Any() == true ? items.ToList() : new List<TModel>();
        }

        /// <summary>
        /// Initializes an <see cref="Pagination{TResult}"/> instance.
        /// </summary>
        /// <param name="currentPage">An <see cref="int"/> value that representing page number that contains these data</param>
        /// <param name="pages">An <see cref="int"/> value that representing page numbers</param>
        /// <param name="countAll">An <see cref="int"/> value that representing loaded data count</param>
        public Pagination(int currentPage, int pages, int countAll = 0) : this(new List<TModel>(), currentPage, pages, countAll)
        {
        }

        /// <summary>
        /// An <see cref="List{T}"/> that representing values being loaded into pagination
        /// </summary>
        public List<TModel> Items { get; set; } = new();

        public int Page { get; set; } = 0;

        public int Pages { get; set; } = 0;

        public int Count { get; set; } = 0;

        public TModel this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }
    }
}