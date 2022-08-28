using FooCommerce.Application.HttpContextRequest;

using Microsoft.AspNetCore.Http;

namespace FooCommerce.Application.Helpers;

public static class HttpContextHelper
{
    public static HttpRequestInfo GetRequestInfo(this HttpContext context) => new(context);
}