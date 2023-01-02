using FooCommerce.CatalogService.Exceptions;
using FooCommerce.CatalogService.Models;

namespace FooCommerce.CatalogService.Services;

public interface ICatalogManager
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="DuplicateCatalogFoundException"></exception>
    /// <exception cref="CatalogNotFoundException"></exception>
    Task CreateAsync(CreateCatalog model, CancellationToken cancellationToken = default);

    /// <summary>
    ///
    /// </summary>
    /// <param name="catalogId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="CatalogNotFoundException"></exception>
    Task DeleteAsync(int catalogId, CancellationToken cancellationToken = default);

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <param name="visible"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="CatalogNotFoundException"></exception>
    Task UpdateVisibilityAsync(int id, bool visible, CancellationToken cancellationToken = default);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="CatalogNotFoundException"></exception>
    /// <exception cref="DuplicateCatalogFoundException"></exception>
    Task UpdateAsync(UpdateCatalog model, CancellationToken cancellationToken = default);

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <param name="order"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="CatalogNotFoundException"></exception>
    Task UpdateOrderAsync(int id, int order, CancellationToken cancellationToken = default);
}