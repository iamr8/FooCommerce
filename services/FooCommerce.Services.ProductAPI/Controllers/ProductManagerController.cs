using System.Net.Mime;

using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.Services.ProductAPI.Controllers;

[Route("api/product/manager")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class ProductManagerController : ControllerBase
{
}