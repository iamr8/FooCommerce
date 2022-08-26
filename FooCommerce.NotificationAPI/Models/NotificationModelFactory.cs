using System.Globalization;
using System.Text;

using FooCommerce.Application.Notifications.Interfaces;
using FooCommerce.Application.Notifications.Models.Contents;
using FooCommerce.Application.Notifications.Models.Types;
using FooCommerce.NotificationAPI.Dtos;
using FooCommerce.NotificationAPI.Extensions;
using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Models.FactoryOptions;

using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Models;

public record NotificationModelFactory : INotificationModelFactory
{
    private readonly INotificationOptions _notificationOptions;
    private readonly ILogger<INotificationModelFactory> _logger;

    private NotificationModelFactory(INotificationOptions notificationOptions, ILoggerFactory loggerFactory)
    {
        _notificationOptions = notificationOptions;
        _logger = loggerFactory.CreateLogger<INotificationModelFactory>();
    }

    public static INotificationModelFactory CreateFactory(INotificationOptions options, ILoggerFactory loggerFactory)
    {
        return new NotificationModelFactory(options, loggerFactory);
    }

    public NotificationPushInAppModel CreatePushInAppModel(NotificationTemplatePushInAppModel template, Action<NotificationPushInAppModelFactoryOptions> options)
    {
        var opt = new NotificationPushInAppModelFactoryOptions();
        options(opt);

        var notificationText = new StringBuilder(template.Message.ToString());
        var notificationSubject = template.Subject.ToString();

        ApplyFormatting(ref notificationText);

        var pushModel = new NotificationPushInAppModel(notificationSubject, notificationText.ToString());
        return pushModel;
    }

    public NotificationSmsModel CreateSmsModel(NotificationTemplateSmsModel template, Action<NotificationSmsModelFactoryOptions> options)
    {
        var opt = new NotificationSmsModelFactoryOptions();
        options(opt);

        var sms = new StringBuilder(template.Text.ToString());

        ApplyFormatting(ref sms);
        ApplyLinks(sms,
            smsLink =>
                $"\r\n{smsLink.Name}\r\n{opt.WebsiteUrl}/{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}{smsLink.Url}");

        var smsModel = new NotificationSmsModel(sms.ToString());
        return smsModel;
    }

    public async Task<NotificationEmailModel> CreateEmailModelAsync(NotificationTemplateEmailModel template, Action<NotificationEmailModelFactoryOptions> options)
    {
        var opt = new NotificationEmailModelFactoryOptions();
        options(opt);

        var emailHtml = new StringBuilder(template.Html.ToString());
        var emailShowRequestData = template.ShowRequestData;

        var htmlLayout = await NotificationTemplateEmailModel.GetLayout(_logger);
        if (string.IsNullOrEmpty(htmlLayout))
            throw new Exception("Unable to find Email Layout.");

        var doc = new HtmlDocument();
        doc.LoadHtml(htmlLayout);

        var footerSocialNetwork = doc.QuerySelector(".js-social-network");
        var footerSocial = footerSocialNetwork.ParentNode;
        footerSocialNetwork = footerSocialNetwork.CloneNode(true);
        footerSocial.ChildNodes.Clear();
        //foreach (var socialMedia in _appSettings.SocialMedias.GetValues())
        //{
        //    var node = footerSocialNetwork.CloneNode(true);
        //    node.Attributes["href"].Value = socialMedia.Url;
        //    node.ChildNodes[0].Attributes["src"].Value = baseUrl + socialMedia.Image;
        //    node.ChildNodes[0].Attributes["alt"].Value = _localizer[socialMedia.Name];
        //    footerSocial.AppendChild(node);
        //}

        ApplyLinks(emailHtml,
            emailButton =>
                $"<p><a href='{opt.WebsiteUrl}/{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}{emailButton.Url}' class='e-button'>{emailButton.Name}</a></p>");

        if (emailShowRequestData)
            await NotificationTemplateEmailModelExtensions.ApplyRequestReplacementsAsync(emailHtml, opt, _logger);

        ApplyFormatting(ref emailHtml);

        doc.QuerySelector(".js-content").InnerHtml = emailHtml.ToString();

        htmlLayout = doc.DocumentNode.OuterHtml;

        NotificationTemplateEmailModel.ApplyLayoutReplacements(ref htmlLayout, _notificationOptions.Receiver.Name, opt);
        NotificationModelFactoryExtensions.ApplyMinification(ref htmlLayout);

        var emailModel = new NotificationEmailModel(new HtmlString(htmlLayout));
        return emailModel;
    }
    private void ApplyLinks(StringBuilder emailHtml, Func<NotificationLink, string> s)
    {
        var links = _notificationOptions.Content.OfType<NotificationLink>();
        if (!links.Any())
            return;

        foreach (var link in links)
        {
            emailHtml.Append(s(link));
        }
    }
    private void ApplyFormatting(ref StringBuilder emailHtml)
    {
        var formattings = _notificationOptions.Content.OfType<NotificationFormatting>();
        if (!formattings.Any())
            return;

        foreach (var formatting in formattings)
        {
            emailHtml = emailHtml.Replace("{{" + formatting.Key + "}}", formatting.Text);
        }
    }
    public NotificationPushModel CreatePushModel(NotificationTemplatePushModel template, Action<NotificationPushModelFactoryOptions> options)
    {
        var opt = new NotificationPushModelFactoryOptions();
        options(opt);

        var notificationText = new StringBuilder(template.Message.ToString());
        var notificationSubject = template.Subject.ToString();

        ApplyFormatting(ref notificationText);

        var pushModel = new NotificationPushModel(notificationSubject, notificationText.ToString());
        return pushModel;
    }
}