using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.CatalogService.Controllers;

[Route("api/products/manager")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class ProductsManagerController : ControllerBase
{
}