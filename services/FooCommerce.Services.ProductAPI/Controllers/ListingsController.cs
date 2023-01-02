using System.Net.Mime;
using FooCommerce.CatalogService.Exceptions;
using FooCommerce.CatalogService.Models;
using FooCommerce.CatalogService.Services;
using FooCommerce.Domain.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.CatalogService.Controllers;

[Route("api/listings")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class ListingsController : Controller
{
    private readonly IListingService _listingService;

    private readonly ILogger<ListingsController> _logger;

    public ListingsController(IListingService listingService, ILogger<ListingsController> logger)
    {
        _listingService = listingService;
        _logger = logger;
    }

    [HttpGet, Route("list")]
    [ProducesResponseType(typeof(PagedList<ListingOverviewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PagedList<ListingOverviewModel>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> List([FromQuery] ProductSearchModel req, CancellationToken cancellationToken = default)
    {
        try
        {
            var model = await _listingService.ToPagedListAsync(req, cancellationToken);
            return Ok(model);
        }
        catch (CatalogNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            return NotFound(PagedList<ListingOverviewModel>.Empty);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}