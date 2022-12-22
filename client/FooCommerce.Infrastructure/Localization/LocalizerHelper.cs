using FooCommerce.Infrastructure.Helpers;
using FooCommerce.Localization;
using Microsoft.AspNetCore.Html;

namespace FooCommerce.Infrastructure.Localization;

public static class LocalizerHelper
{
    /// <summary>
    /// Returns a <see cref="IHtmlContent"/> object that representing replaced input tags with given tags.
    /// </summary>
    /// <param name="localizer">An instance of <see cref="ILocalizer"/>.</param>
    /// <param name="key">Name of specific key in <see cref="Dictionary{TKey,TValue}"/> that containing a localized text.</param>
    /// <param name="tags">A collection of html tags that should be replaced in given html string.</param>
    /// <exception cref="IHtmlContent"></exception>
    /// <returns>A <see cref="IHtmlContent"/> object that representing a formatted html with given tags.</returns>
    public static IHtmlContent Html(this ILocalizer localizer, string key, params Func<string, IHtmlContent>[] tags)
    {
        if (localizer == null)
            throw new ArgumentNullException(nameof(localizer));
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));
        if (tags?.Any() != true)
            throw new ArgumentNullException(nameof(tags));

        var html = localizer[key];
        return HtmlHelper.ReplaceHtml(html, tags);
    }
}