using FooCommerce.Infrastructure.Models;

namespace FooCommerce.Infrastructure.Services;

public interface IListingService
{
    Task<CatalogProductsModel> FindByCategoryAsync(string catalogSlug, int page = 1, CancellationToken cancellationToken = default);

    Task<BrandProductsModel> FindByBrandAsync(string catalogSlug, string brandSlug, int page = 1, CancellationToken cancellationToken = default);

    Task<FoundProductsModel> FindByQueryAsync(string query, int page = 1, CancellationToken cancellationToken = default);
}