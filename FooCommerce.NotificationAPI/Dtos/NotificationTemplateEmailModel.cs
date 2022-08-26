using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

using FooCommerce.Application.Localization.Models;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Notifications.Interfaces;
using FooCommerce.NotificationAPI.Models.FactoryOptions;

using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Dtos;

public record NotificationTemplateEmailModel(Guid Id) : INotificationTemplate
{
    public CommunicationType Communication => CommunicationType.Email_Message;
    public LocalizerValueCollection Html { get; init; }
    public bool ShowRequestData { get; init; }

    public static void ApplyLayoutReplacements(ref string htmlLayout, string receiverName, NotificationEmailModelFactoryOptions options)
    {
        var culture = CultureInfo.CurrentCulture.Equals(CultureInfo.GetCultureInfo("tr"))
            ? CultureInfo.CurrentCulture
            : CultureInfo.GetCultureInfo("en");

        htmlLayout = Regex.Replace(htmlLayout, @"( |\t|\r?\n)\1+", "$1");

        htmlLayout = htmlLayout.Replace("{{baseUrl}}", $"{options.WebsiteUrl}/{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}/{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}");
        htmlLayout = htmlLayout.Replace("{{app_logo}}", $"{options.WebsiteUrl}/img/email/logo.png");

        htmlLayout = htmlLayout.Replace("{{header_pageTitle}}", options.Localizer["AppName"]);
        htmlLayout = htmlLayout.Replace("{{html_lang}}", culture.TwoLetterISOLanguageName);
        htmlLayout = htmlLayout.Replace("{{html_lang_dir}}", culture.TextInfo.IsRightToLeft ? "rtl" : "ltr");

        htmlLayout = htmlLayout.Replace("{{translation_hello}}", options.Localizer["Hi"]);

        htmlLayout = htmlLayout.Replace("{{user_receiverName}}", receiverName);

        htmlLayout = htmlLayout.Replace("{{app_copyright_companyName}}", options.Localizer["ECOHOSCorporation"]);
        htmlLayout = htmlLayout.Replace("{{app_copyright_year}}", options.LocalDateTime.Year.ToString());
        //htmlLayout = htmlLayout.Replace("{{app_copyright_address}}", _appSettings.CompanyInformation.Address);

        htmlLayout = htmlLayout.Replace("{{footer_homepage}}", options.Localizer["Menu_HomePage"]);
        htmlLayout = htmlLayout.Replace("{{footer_unsubscribe}}", options.Localizer["Unsubscribe"]);
        //html = html.Replace("{{footer_unsubscribe_callback}}", unsubscribeToken);
        htmlLayout = htmlLayout.Replace("{{footer_contact}}", options.Localizer["Contact"]);
    }

    public static async Task<string> GetRequestLayoutAsync<T>(ILogger<T> logger)
    {
        var requestDataPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Messaging/Templates/Email/_RequestData.html";
        string requestHtml;
        try
        {
            await using (var fileStream = new FileStream(requestDataPath, FileMode.Open))
            {
                fileStream.Lock(0, fileStream.Length);
                using (var streamReader = new StreamReader(fileStream))
                {
                    requestHtml = await streamReader.ReadToEndAsync();
                }
                fileStream.Unlock(0, fileStream.Length);
            }
        }
        catch (Exception e)
        {
            logger.LogError("Unable to access request data file at path: {0}", requestDataPath);
            return null;
        }

        return requestHtml;
    }
    public static async Task<string> GetLayout<T>(ILogger<T> logger)
    {
        var layoutPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Messaging/Templates/Email/_Layout.html";
        string htmlLayout;
        try
        {
            await using (var fileStreamLayout = new FileStream(layoutPath, FileMode.Open))
            {
                fileStreamLayout.Lock(0, fileStreamLayout.Length);
                using (var streamReader = new StreamReader(fileStreamLayout))
                {
                    htmlLayout = await streamReader.ReadToEndAsync();
                }
                fileStreamLayout.Unlock(0, fileStreamLayout.Length);
            }
        }
        catch (Exception)
        {
            logger.LogError("Unable to access layout file at path: {0}", layoutPath);
            return null;
        }

        return htmlLayout;
    }
}