using System.Text.Json;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FooCommerce.AspNetCoreExtensions.Helpers;

public static class HttpContextHelper
{
    /// <summary>
    /// Adds website endpoint to <see cref="HttpContext"/> for future uses.
    /// </summary>
    /// <remarks>This code must be appended right after <code>app.UseRouting()</code> in <c>Program.cs</c></remarks>
    /// <param name="app"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void UseWebsiteUrl(ref WebApplication app)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));
        app.Use(async (context, next) =>
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                context.Items.Add("WebsiteURL", endpoint.Metadata);
            }

            await next();
        });
    }

    /// <summary>
    /// Returns endpoint url of the web app.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public static string GetWebsiteUrl(this HttpContext httpContext)
    {
        if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

        var scheme = httpContext.Request.Scheme;
        var webEnvironment = httpContext.RequestServices.GetService<IWebHostEnvironment>();
        if (webEnvironment.IsDevelopment())
            return $"{scheme}://{httpContext.Request.Host}{httpContext.Request.PathBase}";

        if (httpContext.Request.Headers.TryGetValue("CF-Visitor", out var cloudFlareVisitor))
        {
            scheme = JsonSerializer.Deserialize<dynamic>(cloudFlareVisitor)!.scheme;
        }
        else
        {
            if (httpContext.Request.Headers.TryGetValue("X-Forwarded-Proto", out var xForwarderProto))
            {
                scheme = xForwarderProto;
            }
        }

        var hasWebsiteUrl = httpContext.Items.TryGetValue("WebsiteURL", out var websiteUrl);
        if (!hasWebsiteUrl)
            throw new NullReferenceException("Unable to get Website url from HttpContext.");

        return $"{scheme}://{(webEnvironment.IsProduction() ? "www." : "")}{websiteUrl}/";
    }
}