using FooCommerce.Domain.Pagination;
using FooCommerce.Infrastructure.Models;

namespace FooCommerce.Infrastructure.Services.Microservices;

public interface ICatalogClient
{
    /// <summary>
    /// Returns a catalog's information.
    /// </summary>
    /// <param name="slug"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException"></exception>
    Task<CatalogOverviewModel> GetCatalogOverviewAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a catalog's products.
    /// </summary>
    /// <param name="catalogId"></param>
    /// <param name="cancellationToken"></param>
    Task<PagedList<ProductOverviewModel>> GetProductsOverviewAsync(int catalogId, CancellationToken cancellationToken = default);
}