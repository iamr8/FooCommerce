using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;

namespace FooCommerce.Infrastructure.Mvc.Localization
{
    public class RouteValueRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            string cultureCode = null;
            var culture = httpContext.GetRouteValue(Constraints.LanguageKey)?.ToString();

            switch (httpContext.Request.Path.HasValue)
            {
                case true when httpContext.Request.Path.Value == "/":
                    cultureCode = GetDefaultCultureCode();
                    break;

                case true when httpContext.Request.Path.Value.Length >= 4 && httpContext.Request.Path.Value[0] == '/' && httpContext.Request.Path.Value[3] == '/':
                    {
                        cultureCode = httpContext.Request.Path.Value.Substring(1, 2);

                        if (!CheckCultureCode(cultureCode))
                            cultureCode = GetDefaultCultureCode(); //throw new HttpException(HttpStatusCode.NotFound);
                        break;
                    }

                default:
                    cultureCode = GetDefaultCultureCode(); //throw new HttpException(HttpStatusCode.NotFound);
                    break;
            }

            var requestCulture = new ProviderCultureResult(cultureCode);
            return Task.FromResult(requestCulture);
        }

        private string GetDefaultCultureCode()
        {
            return Options.DefaultRequestCulture.Culture.TwoLetterISOLanguageName;
        }

        private bool CheckCultureCode(string cultureCode)
        {
            return Options.SupportedCultures.Select(c => c.TwoLetterISOLanguageName).Contains(cultureCode);
        }
    }
}