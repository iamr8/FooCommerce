using System.Globalization;
using System.Reflection;
using System.Text;

using FooCommerce.Domain.ContextRequest;
using FooCommerce.Localization;
using FooCommerce.Localization.Helpers;
using FooCommerce.Services.NotificationAPI.Contracts;
using FooCommerce.Services.NotificationAPI.Dtos;
using FooCommerce.Services.NotificationAPI.Enums;
using FooCommerce.Services.NotificationAPI.Interfaces;
using FooCommerce.Services.NotificationAPI.Services.Repositories;

using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

using MassTransit;

using WebMarkupMin.Core;

namespace FooCommerce.Services.NotificationAPI.Handlers;

public class EmailHandler : MessageHandler
{
    private readonly ILogger<EmailHandler> _logger;

    public EmailHandler(ILocalizer localizer, IBus bus, ILogger<EmailHandler> logger) : base(bus, localizer)
    {
        _logger = logger;
    }

    public override async Task EnqueueAsync(ITemplate template,
        NotificationPurpose purpose,
        string receiverName,
        string receiverAddress,
        IDictionary<string, string> links,
        IDictionary<string, string> formatters,
        string websiteUrl,
        ContextRequestInfo requestInfo,
        CancellationToken cancellationToken = default)
    {
        if (template == null)
            throw new ArgumentNullException(nameof(template));
        if (receiverName == null)
            throw new ArgumentNullException(nameof(receiverName));
        if (receiverAddress == null)
            throw new ArgumentNullException(nameof(receiverAddress));
        if (links == null) throw
            new ArgumentNullException(nameof(links));
        if (formatters == null)
            throw new ArgumentNullException(nameof(formatters));
        if (websiteUrl == null)
            throw new ArgumentNullException(nameof(websiteUrl));
        if (requestInfo == null)
            throw new ArgumentNullException(nameof(requestInfo));
        if (template is not EmailTemplateModel emailTemplate)
            throw new ArgumentException("Template is not Email template", nameof(template));

        var emailHtml = new StringBuilder(emailTemplate.Html.ToString());
        var emailShowRequestData = emailTemplate.ShowRequestData;

        var htmlLayout = await ReadAsync(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Templates/_Layout.html");
        if (string.IsNullOrEmpty(htmlLayout))
            throw new Exception("Unable to find Email Layout.");

        var doc = new HtmlDocument();
        doc.LoadHtml(htmlLayout);
        var dict = new Dictionary<string, string>();

        //var footerSocialNetwork = doc.QuerySelector(".js-social-network");
        //var footerSocial = footerSocialNetwork.ParentNode;
        //footerSocialNetwork = footerSocialNetwork.CloneNode(true);
        //footerSocial.ChildNodes.Clear();
        //foreach (var socialMedia in _appSettings.SocialMedias.GetValues())
        //{
        //    var node = footerSocialNetwork.CloneNode(true);
        //    node.Attributes["href"].Value = socialMedia.Url;
        //    node.ChildNodes[0].Attributes["src"].Value = baseUrl + socialMedia.Image;
        //    node.ChildNodes[0].Attributes["alt"].Value = Localizer[socialMedia.Name];
        //    footerSocial.AppendChild(node);
        //}

        var localTime = DateTime.UtcNow.ToLocal(requestInfo);
        Coordinator.ApplyLinks(links, emailHtml,
            emailButton => $"<p><a href='{websiteUrl}/{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}{emailButton.Value}' class='e-button'>{emailButton.Key}</a></p>");

        var requestHtml = await ReadAsync(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Templates/_RequestData.html");
        if (string.IsNullOrEmpty(requestHtml))
            throw new Exception("Unable to find Request Html Layout.");

        Coordinator.ApplyFormatters(formatters, ref emailHtml);

        doc.QuerySelector(".js-content").InnerHtml = emailHtml.ToString();
        htmlLayout = doc.DocumentNode.OuterHtml;

        GetLayoutDictionary(dict, receiverName, websiteUrl, localTime, requestInfo.Culture);
        if (emailShowRequestData)
            GetRequestDictionary(dict, websiteUrl, requestInfo);

        ApplyMinification(ref htmlLayout);

        var htmlBuilder = new StringBuilder(htmlLayout);
        foreach (var (key, value) in dict)
        {
            htmlBuilder.Replace(key, value);
        }

        await Bus.Publish<EnqueueEmail>(new
        {
            IsImportant = true, // Must check from purpose
            Subject = Localizer[purpose.GetLocalizer()],
            Body = htmlBuilder.ToString(),
            ReceiverName = receiverName,
            ReceiverAddress = receiverAddress,
        }, cancellationToken);
    }

    private static void ApplyMinification(ref string htmlLayout)
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

    private void GetRequestDictionary(IDictionary<string, string> dict, string websiteUrl, ContextRequestInfo requestInfo)
    {
        var ipAddress = requestInfo.IPAddress;
        var country = requestInfo.Country;
        var platform = $"{requestInfo.Platform.Name} {requestInfo.Platform.Version}";
        var browser = $"{requestInfo.Browser.Name} {requestInfo.Browser.Version}";

        dict.Add("{{request_ip}}", ipAddress?.ToString());
        dict.Add("{{request_location}}", country != null ? $"({country.EnglishName})" : "");
        dict.Add("{{request_platform}}", platform);
        dict.Add("{{request_browser}}", browser);

        dict.Add("{{translation_ip}}", Localizer["IPAddress"]);
        dict.Add("{{translation_platform}}", Localizer["OperatingSystem"]);
        dict.Add("{{translation_browser}}", Localizer["WebBrowser"]);

        var resetPasswordUrl = $"{websiteUrl}/{requestInfo.Culture.TwoLetterISOLanguageName}/account/password/reset";
        dict.Add("{{translation_caution}}", Localizer.Format("Account_Email_IfYouDoNotRecognizeThisActivity", $"<a href=\"{resetPasswordUrl}\" class=\"e-link js-mail-link\">{Localizer["Account_ResetPassword"]}</a>"));
    }

    private void GetLayoutDictionary(IDictionary<string, string> dict, string receiverName, string websiteUrl, DateTime localDateTime, CultureInfo culture)
    {
        dict.Add("{{baseUrl}}", $"{websiteUrl}/{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}/{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}");
        dict.Add("{{app_logo}}", $"{websiteUrl}/img/email/logo.png");

        dict.Add("{{header_pageTitle}}", Localizer["AppName"]);
        dict.Add("{{html_lang}}", culture.TwoLetterISOLanguageName);
        dict.Add("{{html_lang_dir}}", culture.TextInfo.IsRightToLeft ? "rtl" : "ltr");

        dict.Add("{{translation_hello}}", Localizer["Hi"]);

        dict.Add("{{user_receiverName}}", receiverName);

        dict.Add("{{app_copyright_companyName}}", Localizer["ECOHOSCorporation"]);
        dict.Add("{{app_copyright_year}}", localDateTime.Year.ToString());
        //dict.Add("{{app_copyright_address}}", _appSettings.CompanyInformation.Address);

        dict.Add("{{footer_homepage}}", Localizer["Menu_HomePage"]);
        dict.Add("{{footer_unsubscribe}}", Localizer["Unsubscribe"]);
        //html = html.Replace("{{footer_unsubscribe_callback}}", unsubscribeToken);
        dict.Add("{{footer_contact}}", Localizer["Contact"]);
    }

    private async Task<string> ReadAsync(string path)
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
            _logger.LogError("Unable to access layout file at path: {0}", path);
            return null;
        }

        return output;
    }
}