using System.Net.Mime;

using FooCommerce.Infrastructure.Models;
using FooCommerce.Infrastructure.Services;

using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.WebApi.Controllers;

[Route("search")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class SearchController : ControllerBase
{
    private readonly IListingService _listingService;

    public SearchController(IListingService listingService)
    {
        _listingService = listingService;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="query"></param>
    /// <param name="queries"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet, Route("index")]
    [ProducesResponseType(typeof(FoundProductsModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<SearchModel>> SearchByQueryAsync([FromQuery(Name = "q")] string query,
        [FromQuery] IDictionary<string, string> queries = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var queryCollection = Request.Query;
            var model = await _listingService.FindByQueryAsync(query, 1, cancellationToken);
            return Ok(model);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="catalogSlug"></param>
    /// <param name="queries"></param>
    /// <param name="cancellationToken"></param>
    [HttpGet, Route("{catalog}")]
    [ProducesResponseType(typeof(CatalogProductsModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<SearchModel>> SearchByCategoryAsync([FromRoute(Name = "catalog")] string catalogSlug,
        [FromQuery] IDictionary<string, string> queries = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var queryCollection = Request.Query;
            var model = await _listingService.FindByCategoryAsync(catalogSlug, 1, cancellationToken);
            return Ok(model);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="catalogSlug"></param>
    /// <param name="brandSlug"></param>
    /// <param name="queries"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet, Route("{catalog}/{brand}")]
    [ProducesResponseType(typeof(BrandProductsModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<SearchModel>> SearchByBrandAsync([FromRoute(Name = "catalog")] string catalogSlug,
        [FromRoute(Name = "brand")] string brandSlug,
        [FromQuery] IDictionary<string, string> queries = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var queryCollection = Request.Query;
            var model = await _listingService.FindByBrandAsync(catalogSlug, brandSlug, 1, cancellationToken);
            return Ok(model);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}