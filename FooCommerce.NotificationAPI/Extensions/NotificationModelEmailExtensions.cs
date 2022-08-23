using System.Globalization;
using System.Text;

using FooCommerce.Application.Dtos.Notifications;
using FooCommerce.Application.Models.Notifications.Options;
using FooCommerce.Infrastructure.Helpers;
using FooCommerce.Infrastructure.Localization;

using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Extensions;

public static class NotificationTemplateEmailModelExtensions
{
    public static async Task ApplyRequestReplacementsAsync<T>(StringBuilder emailHtml, NotificationEmailModelOptions options, ILogger<T> logger)
    {
        var requestHtml = await NotificationTemplateEmailModel.GetRequestLayoutAsync(logger);
        if (string.IsNullOrEmpty(requestHtml))
            throw new Exception("Unable to find Request Html Layout.");

        requestHtml = requestHtml.Replace("{{request_ip}}", options.IPAddress?.ToString());
        requestHtml = requestHtml.Replace("{{request_location}}", options.Country != null ? $"({options.Country.EnglishName})" : "");
        requestHtml = requestHtml.Replace("{{request_platform}}", options.Platform);
        requestHtml = requestHtml.Replace("{{request_browser}}", options.Browser);

        requestHtml = requestHtml.Replace("{{translation_ip}}", options.Localizer["IPAddress"]);
        requestHtml = requestHtml.Replace("{{translation_platform}}", options.Localizer["OperatingSystem"]);
        requestHtml = requestHtml.Replace("{{translation_browser}}", options.Localizer["WebBrowser"]);

        var resetPasswordUrl = $"{options.WebsiteUrl}/{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}/account/password/reset";
        requestHtml = requestHtml.Replace("{{translation_caution}}", options.Localizer.Html("Account_Email_IfYouDoNotRecognizeThisActivity", c => new HtmlString($"<a href=\"{resetPasswordUrl}\" class=\"e-link js-mail-link\">{options.Localizer["Account_ResetPassword"]}</a>")).GetString());

        emailHtml.Append(requestHtml);
    }
}