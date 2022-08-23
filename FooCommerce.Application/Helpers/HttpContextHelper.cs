using FooCommerce.Application.Models;

using Microsoft.AspNetCore.Http;

namespace FooCommerce.Application.Helpers;

public static class HttpContextHelper
{
    public static EndUser GetEndUser(this HttpContext context) => new(context);
}