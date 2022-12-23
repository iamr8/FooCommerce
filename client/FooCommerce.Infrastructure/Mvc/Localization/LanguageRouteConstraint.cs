using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Mvc.Localization;

public class LanguageRouteConstraint : IRouteConstraint
{
    public bool Match(HttpContext httpContext,
        IRouter route,
        string routeKey,
        RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        if (routeKey == null)
        {
            throw new ArgumentNullException(nameof(routeKey));
        }

        if (values == null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        var configuration = httpContext.RequestServices.GetService<IConfiguration>();
        var supportedLanguages = configuration!.GetSection("SupportedLanguages").Get<string[]>();

        var page = string.Empty;
        var controller = string.Empty;
        var action = string.Empty;
        var area = string.Empty;
        if (values.ContainsKey("page"))
        {
            page = values["page"].ToString();
        }
        else if (values.ContainsKey("action") && values.ContainsKey("controller"))
        {
            controller = values["controller"].ToString();
            action = values["action"].ToString();
        }

        if (values.ContainsKey("area") && !string.IsNullOrEmpty(values["area"].ToString()))
            area = values["area"].ToString();

        if (!values.ContainsKey(LanguageConstraints.LanguageKey))
            return false;

        var lang = values[LanguageConstraints.LanguageKey].ToString();
        if (supportedLanguages.Any(x => x.Equals(lang)))
            return true;

        if (values.ContainsKey("page"))
            values["page"] = $"/{lang}{values["page"]}";

        values[LanguageConstraints.LanguageKey] = supportedLanguages[0];
        return false;
    }
}