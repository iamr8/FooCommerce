using System.Net.Mime;

using FooCommerce.CatalogService.Exceptions;
using FooCommerce.CatalogService.Models;
using FooCommerce.CatalogService.Services;

using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.CatalogService.Controllers;

[Route("api/catalogs")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class CatalogsController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    private readonly ILogger<CatalogsManagerController> _logger;

    public CatalogsController(ICatalogService catalogService, ILogger<CatalogsManagerController> logger)
    {
        _logger = logger;
        _catalogService = catalogService;
    }

    /// <summary>
    /// Returns a catalog's overview.
    /// </summary>
    /// <param name="slug"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Returns catalog information.</response>
    /// <response code="404">Catalog not found.</response>
    /// <response code="500">An internal server error occurred.</response>
    [HttpGet, Route("overview")]
    [ProducesResponseType(typeof(CatalogOverviewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOverview(string slug, CancellationToken cancellationToken = default)
    {
        try
        {
            var model = await _catalogService.GetAsync(slug, cancellationToken);
            return Ok(model);
        }
        catch (CatalogNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}