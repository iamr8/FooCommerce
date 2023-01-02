using System.Net.Mime;

using FooCommerce.CatalogService.Exceptions;
using FooCommerce.CatalogService.Models;
using FooCommerce.CatalogService.RestModels;
using FooCommerce.CatalogService.Services;

using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.CatalogService.Controllers;

[Route("api/catalogs/manager")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class CatalogsManagerController : ControllerBase
{
    private readonly ICatalogManager _catalogManager;

    private readonly ILogger<CatalogsManagerController> _logger;

    public CatalogsManagerController(ICatalogManager catalogManager, ILogger<CatalogsManagerController> logger)
    {
        _logger = logger;
        _catalogManager = catalogManager;
    }

    /// <summary>
    /// Creates a new catalog.
    /// </summary>
    /// <param name="req"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="201">Catalog has been created.</response>
    /// <response code="404">Parent catalog not found.</response>
    /// <response code="409">Catalog already exists.</response>
    /// <response code="500">An internal server error occurred.</response>
    [HttpPost, Route("index")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(CreateCatalogReq req, CancellationToken cancellationToken = default)
    {
        try
        {
            await _catalogManager.CreateAsync(new CreateCatalog
            {
                Name = req.Name,
                Description = req.Description,
                IconPath = req.Icon,
                ParentId = req.ParentId
            }, cancellationToken);
            return Created("api/catalog/manager/index", new { });
        }
        catch (DuplicateCatalogFoundException e)
        {
            _logger.LogError(e, e.Message);
            return Conflict();
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

    /// <summary>
    /// Deletes a catalog.
    /// </summary>
    /// <param name="req"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Catalog has been deleted.</response>
    /// <response code="404">Catalog not found.</response>
    /// <response code="500">An internal server error occurred.</response>
    [HttpDelete, Route("index")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(DeleteCatalogReq req, CancellationToken cancellationToken = default)
    {
        try
        {
            await _catalogManager.DeleteAsync(req.CatalogId, cancellationToken);
            return NoContent();
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

    /// <summary>
    /// Updated a catalog.
    /// </summary>
    /// <param name="req"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Catalog has been updated.</response>
    /// <response code="404">Parent catalog not found.</response>
    /// <response code="409">Catalog already exists.</response>
    /// <response code="500">An internal server error occurred.</response>
    [HttpPut, Route("index")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(UpdateCatalogReq req, CancellationToken cancellationToken = default)
    {
        try
        {
            await _catalogManager.UpdateAsync(new UpdateCatalog
            {
                Id = req.Id,
                Name = req.Name,
                Description = req.Description,
                IconPath = req.Icon,
                ParentId = req.ParentId
            }, cancellationToken);
            return NoContent();
        }
        catch (DuplicateCatalogFoundException e)
        {
            _logger.LogError(e, e.Message);
            return Conflict();
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

    /// <summary>
    /// Updates a catalog order.
    /// </summary>
    /// <param name="req"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Catalog's order has been updated.</response>
    /// <response code="404">Catalog not found.</response>
    /// <response code="500">An internal server error occurred.</response>
    [HttpPut, Route("update-order")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateOrder(UpdateCatalogOrderReq req, CancellationToken cancellationToken = default)
    {
        try
        {
            await _catalogManager.UpdateOrderAsync(req.Id, req.Order, cancellationToken);
            return NoContent();
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

    /// <summary>
    /// Updates a catalog visibility.
    /// </summary>
    /// <param name="req"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Catalog's visibility has been updated.</response>
    /// <response code="404">Catalog not found.</response>
    /// <response code="500">An internal server error occurred.</response>
    [HttpPut, Route("update-visibility")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateVisibility(UpdateCatalogVisibilityReq req, CancellationToken cancellationToken = default)
    {
        try
        {
            await _catalogManager.UpdateVisibilityAsync(req.Id, default, cancellationToken);
            return NoContent();
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