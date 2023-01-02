using FooCommerce.CatalogService.Models;
using FooCommerce.Domain.Pagination;

namespace FooCommerce.CatalogService.Services;

public interface IListingService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="search"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    Task<PagedList<ListingOverviewModel>> ToPagedListAsync(ProductSearchModel search, CancellationToken cancellationToken = default);
}