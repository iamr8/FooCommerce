using FooCommerce.Infrastructure.Models;
using FooCommerce.Infrastructure.Services.Microservices;

namespace FooCommerce.Infrastructure.Services.Repositories;

public class ListingService : IListingService
{
    private readonly ICatalogClient _catalogClient;

    public ListingService(ICatalogClient catalogClient)
    {
        _catalogClient = catalogClient;
    }

    public async Task<CatalogProductsModel> FindByCategoryAsync(
        string catalogSlug,
        int page = 1,
        CancellationToken cancellationToken = default)
    {
        var catalog = await _catalogClient.GetCatalogOverviewAsync(catalogSlug, cancellationToken);
        var products = await _catalogClient.GetProductsOverviewAsync(catalog.Id, cancellationToken);
        // TODO: find a mechanism to get brand name

        var model = new CatalogProductsModel
        {
            Catalog = catalog,
            Products = products.Items,
            Pager = new PaginationModel
            {
                Page = products.PageNo,
                TotalItems = products.TotalCount,
                TotalPages = products.Pages
            }
        };
        return model;
    }

    public Task<BrandProductsModel> FindByBrandAsync(string catalogSlug, string brandSlug, int page = 1,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FoundProductsModel> FindByQueryAsync(string query, int page = 1, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    //public async Task<SearchModel> SearchAsync(string query, int page = 1, CancellationToken cancellationToken = default)
    //{
    //    var products = await _catalogClient.SearchProductsAsync(query, cancellationToken);

    //    var model = new SearchModel
    //    {
    //        Query = query,
    //        Products = products,
    //    };
    //    return model;
    //}
}