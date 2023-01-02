using System.Linq.Expressions;
using FooCommerce.Domain.Pagination;
using Microsoft.EntityFrameworkCore;

namespace FooCommerce.EntityFrameworkCore.Extensions.Pagination;

public static class PagedListHelper
{
    /// <summary>
    /// Paginates results according to given page number and page size.
    /// </summary>
    /// <typeparam name="TSource">A generic type of <see cref="EntityAuditable"/>.</typeparam>
    /// <typeparam name="TResult">A generic type for result.</typeparam>
    /// <param name="query">A <see cref="IQueryable{T}"/> that representing source query.</param>
    /// <param name="selector">An <see cref="Expression{TResult}"/> that projecting results.</param>
    /// <param name="page">An <see cref="int"/> value that representing specific page of items.</param>
    /// <param name="pageSize">An <see cref="int"/> value that representing size of pages.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>A <see cref="PagedList{TModel}"/> instance that representing query results divided in each pages.</returns>
    public static async Task<PagedList<TResult>> ToPagedListAsync<TSource, TResult>(
        this IQueryable<TSource> query,
        Expression<Func<TSource, TResult>> selector,
        int page,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
        where TSource : class
        where TResult : class
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        var output = new PagedList<TResult>();

        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var rowCount = await query
            .CountAsync(cancellationToken: cancellationToken);
        if (rowCount <= 0)
            return output;

        var pageCount = Math.Ceiling((double)rowCount / pageSize);
        if (page > pageCount)
            page = 1;

        if (page > 1)
            query = query.Skip(pageSize * (page - 1));

        var models = await query
            .Take(pageSize)
            .Select(selector)
            .ToListAsync(cancellationToken: cancellationToken);

        var pages = Math.Ceiling((double)rowCount / pageSize);
        output = new PagedList<TResult>(models, page, Convert.ToInt32(pages), rowCount);
        return output;
    }
}