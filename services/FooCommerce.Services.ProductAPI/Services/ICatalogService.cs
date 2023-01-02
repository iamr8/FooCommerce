using FooCommerce.CatalogService.Exceptions;
using FooCommerce.CatalogService.Models;

namespace FooCommerce.CatalogService.Services;

public interface ICatalogService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="slug"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="CatalogNotFoundException"></exception>
    Task<CatalogOverviewModel> GetAsync(string slug, CancellationToken cancellationToken = default);
}