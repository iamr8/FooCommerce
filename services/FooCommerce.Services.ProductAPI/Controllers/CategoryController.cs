using System.Net.Mime;

using FooCommerce.Services.ProductAPI.Exceptions;
using FooCommerce.Services.ProductAPI.Models;
using FooCommerce.Services.ProductAPI.Services;

using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.Services.ProductAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class CategoryController : ControllerBase
{
    private readonly ICategoryManager _categoryManager;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryManager categoryManager, ILogger<CategoryController> logger)
    {
        _logger = logger;
        _categoryManager = categoryManager;
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <param name="req"></param>
    /// <param name="cancellationToken"></param>
    [HttpPost, Route("create")]
    [ProducesResponseType(typeof(CreateCategoryResp), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(CreateCategoryRespFaulted), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CreateCategoryRespFaulted), StatusCodes.Status500InternalServerError)]
    public async Task<ICreateCategoryResp> Create(CreateCategoryReq req, CancellationToken cancellationToken = default)
    {
        try
        {
            await _categoryManager.CreateAsync(req.GroupId, req.Name, req.Description, req.Icon, req.ParentId, cancellationToken);
            this.Response.StatusCode = StatusCodes.Status201Created;
            return new CreateCategoryResp();
        }
        catch (DuplicateCategoryFoundException e)
        {
            _logger.LogError(e, e.Message);
            this.Response.StatusCode = StatusCodes.Status400BadRequest;
            return new CreateCategoryRespFaulted { Status = CreateCategoryFaults.AlreadyExists };
        }
        catch (ParentCategoryNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            this.Response.StatusCode = StatusCodes.Status400BadRequest;
            return new CreateCategoryRespFaulted { Status = CreateCategoryFaults.ParentNotFound };
        }
        catch (CategoryGroupNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            this.Response.StatusCode = StatusCodes.Status400BadRequest;
            return new CreateCategoryRespFaulted { Status = CreateCategoryFaults.GroupNotFound };
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            this.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return new CreateCategoryRespFaulted();
        }
    }
}