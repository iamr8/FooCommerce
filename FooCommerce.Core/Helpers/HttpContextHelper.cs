using FooCommerce.Core.HttpContextRequest;

using Microsoft.AspNetCore.Http;

namespace FooCommerce.Core.Helpers;

public static class HttpContextHelper
{
    public static HttpRequestInfo GetRequestInfo(this HttpContext context) => new(context);
}