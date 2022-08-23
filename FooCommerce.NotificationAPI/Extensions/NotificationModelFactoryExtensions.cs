using System.Text;

using WebMarkupMin.Core;

namespace FooCommerce.NotificationAPI.Extensions;

public static class NotificationModelFactoryExtensions
{
    public static void ApplyMinification(ref string htmlLayout)
    {
        var settings = new HtmlMinificationSettings
        {
            MinifyEmbeddedCssCode = true,
            MinifyEmbeddedJsCode = true,
            MinifyInlineJsCode = true,
            MinifyInlineCssCode = true,
            RemoveHtmlComments = true,
            RemoveOptionalEndTags = true,
        };
        var cssMinifier = new KristensenCssMinifier();
        var jsMinifier = new CrockfordJsMinifier();
        var logger = new WebMarkupMin.Core.Loggers.NullLogger();
        var htmlMinifier = new HtmlMinifier(settings, cssMinifier, jsMinifier, logger);
        var minifiedHtmlFactory = htmlMinifier.Minify(htmlLayout,
            fileContext: string.Empty,
            encoding: Encoding.GetEncoding(0),
            generateStatistics: false);
        htmlLayout = minifiedHtmlFactory.MinifiedContent;
    }
}