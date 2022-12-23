using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.CatalogService.Controllers;

[Route("api/product/manager")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class ProductManagerController : ControllerBase
{
}