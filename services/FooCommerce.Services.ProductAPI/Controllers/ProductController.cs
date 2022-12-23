using System.Net.Mime;

using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.Services.ProductAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class ProductController : ControllerBase
{
}