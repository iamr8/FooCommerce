using System.Globalization;
using System.Reflection;

using FooCommerce.Application.Localization.Models;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Notifications.Interfaces;
using FooCommerce.Core.Helpers;
using FooCommerce.Core.HttpContextRequest;
using FooCommerce.Core.Localization.Helpers;
using FooCommerce.Domain.Interfaces;
using FooCommerce.NotificationAPI.Models.FactoryOptions;

using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Dtos;

public record NotificationTemplateEmailModel : INotificationTemplate
{
    internal NotificationTemplateEmailModel(Guid id)
    {
        this.Id = id;
    }
    public Guid Id { get; }
    public CommunicationType Communication => CommunicationType.Email_Message;
    public LocalizerValueCollection Html { get; init; }
    public bool ShowRequestData { get; init; }
    public IDictionary<string, string> Values { get; }

    public static void GetRequestDictionary(IDictionary<string, string> dict, NotificationEmailModelFactoryOptions options, IHttpRequestInfo requestInfo, ILocalizer localizer)
    {
        var ipAddress = requestInfo.IPAddress;
        var country = requestInfo.Country;
        var platform = $"{requestInfo.Platform.Name} {requestInfo.Platform.Version}";
        var browser = $"{requestInfo.Browser.Name} {requestInfo.Browser.Version}";

        dict.Add("{{request_ip}}", ipAddress?.ToString());
        dict.Add("{{request_location}}", country != null ? $"({country.EnglishName})" : "");
        dict.Add("{{request_platform}}", platform);
        dict.Add("{{request_browser}}", browser);

        dict.Add("{{translation_ip}}", localizer["IPAddress"]);
        dict.Add("{{translation_platform}}", localizer["OperatingSystem"]);
        dict.Add("{{translation_browser}}", localizer["WebBrowser"]);

        var resetPasswordUrl = $"{options.WebsiteUrl}/{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}/account/password/reset";
        dict.Add("{{translation_caution}}", localizer.Html("Account_Email_IfYouDoNotRecognizeThisActivity", c => new HtmlString($"<a href=\"{resetPasswordUrl}\" class=\"e-link js-mail-link\">{localizer["Account_ResetPassword"]}</a>")).GetString());
    }

    public static void GetLayoutDictionary(Dictionary<string, string> dict, string receiverName, NotificationEmailModelFactoryOptions options, ILocalizer localizer)
    {
        var culture = CultureInfo.CurrentCulture.Equals(CultureInfo.GetCultureInfo("tr"))
            ? CultureInfo.CurrentCulture
            : CultureInfo.GetCultureInfo("en");

        dict.Add("{{baseUrl}}", $"{options.WebsiteUrl}/{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}/{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}");
        dict.Add("{{app_logo}}", $"{options.WebsiteUrl}/img/email/logo.png");

        dict.Add("{{header_pageTitle}}", localizer["AppName"]);
        dict.Add("{{html_lang}}", culture.TwoLetterISOLanguageName);
        dict.Add("{{html_lang_dir}}", culture.TextInfo.IsRightToLeft ? "rtl" : "ltr");

        dict.Add("{{translation_hello}}", localizer["Hi"]);

        dict.Add("{{user_receiverName}}", receiverName);

        dict.Add("{{app_copyright_companyName}}", localizer["ECOHOSCorporation"]);
        dict.Add("{{app_copyright_year}}", options.LocalDateTime.Year.ToString());
        //dict.Add("{{app_copyright_address}}", _appSettings.CompanyInformation.Address);

        dict.Add("{{footer_homepage}}", localizer["Menu_HomePage"]);
        dict.Add("{{footer_unsubscribe}}", localizer["Unsubscribe"]);
        //html = html.Replace("{{footer_unsubscribe_callback}}", unsubscribeToken);
        dict.Add("{{footer_contact}}", localizer["Contact"]);
    }

    public static Task<string> GetRequestLayoutAsync<T>(ILogger<T> logger)
    {
        var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Templates/_RequestData.html";
        var output = ReadFile(path, logger);
        return output;
    }

    private static async Task<string> ReadFile<TLogger>(string path, ILogger<TLogger> logger)
    {
        string output;
        try
        {
            await using var fileStream = new FileStream(path, FileMode.Open);
            try
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                fileStream.Lock(0, fileStream.Length);
                using var streamReader = new StreamReader(fileStream);
                output = await streamReader.ReadToEndAsync();
                streamReader.Dispose();
            }
            finally
            {
                if (fileStream.CanRead || fileStream.CanSeek)
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    fileStream.Unlock(0, fileStream.Length);
                }

                await fileStream.DisposeAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Unable to access layout file at path: {0}", path);
            return null;
        }

        return output;
    }
    public static Task<string> GetLayout<TLogger>(ILogger<TLogger> logger)
    {
        var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Templates/_Layout.html";
        var output = ReadFile(path, logger);
        return output;
    }
}