using FooCommerce.Common.HttpContextRequest;

using Microsoft.AspNetCore.Http;

namespace FooCommerce.Common.Helpers;

public static class HttpContextHelper
{
    public static IHttpRequestInfo GetRequestInfo(this HttpContext context) => new HttpRequestInfo(context);
}