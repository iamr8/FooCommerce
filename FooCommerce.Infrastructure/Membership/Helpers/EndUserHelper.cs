using System.Text.Json;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FooCommerce.Infrastructure.Membership.Helpers;

public static class EndUserHelper
{
    public static string GetWebsiteUrl(this HttpContext httpContext, bool forceRemoteUrl = false, string fallbackDevUrl = "localhost:8080")
    {
        var scheme = httpContext.Request.Scheme;

        if (httpContext.Request.Headers.TryGetValue("CF-Visitor", out var cloudFlareVisitor))
        {
            var json = JsonSerializer.Deserialize<dynamic>(cloudFlareVisitor);
            scheme = json.scheme;
        }
        else
        {
            if (httpContext.Request.Headers.TryGetValue("X-Forwarded-Proto", out var xForwarderProto))
            {
                scheme = xForwarderProto;
            }
        }

        var configuration = httpContext.RequestServices.GetService<IConfiguration>();
        var webEnvironment = httpContext.RequestServices.GetService<IWebHostEnvironment>();
        var webSiteURL = configuration!["WebsiteURL"];

        scheme ??= "http";
        string url;
        if (forceRemoteUrl)
        {
            url = $"{scheme}://{webSiteURL}/";
        }
        else
        {
            var host = !webEnvironment.IsProduction()
                ? !string.IsNullOrEmpty(fallbackDevUrl) ? fallbackDevUrl : httpContext.Request.Host.Value
                : webSiteURL;

            url = $"{scheme}://{(webEnvironment.IsProduction() ? "www." : "")}{host}/";
        }

        return url;
    }
}