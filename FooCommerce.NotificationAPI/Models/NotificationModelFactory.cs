﻿using System.Globalization;
using System.Text;

using FooCommerce.Domain.Interfaces;
using FooCommerce.NotificationAPI.Bridge.Interfaces;
using FooCommerce.NotificationAPI.Bridge.Models;
using FooCommerce.NotificationAPI.Dtos;
using FooCommerce.NotificationAPI.Extensions;
using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Models.FactoryOptions;
using FooCommerce.NotificationAPI.Models.Types;

using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace FooCommerce.NotificationAPI.Models;

public record NotificationModelFactory : INotificationModelFactory
{
    private readonly INotificationOptions _notificationOptions;
    private readonly ILocalizer _localizer;
    private readonly ILogger<INotificationModelFactory> _logger;

    private NotificationModelFactory(INotificationOptions notificationOptions, ILoggerFactory loggerFactory, ILocalizer localizer)
    {
        _notificationOptions = notificationOptions;
        _localizer = localizer;
        _logger = loggerFactory.CreateLogger<INotificationModelFactory>();
    }

    public static INotificationModelFactory CreateFactory(INotificationOptions options, ILoggerFactory loggerFactory, ILocalizer localizer)
    {
        return new NotificationModelFactory(options, loggerFactory, localizer);
    }

    public NotificationPushInAppModel CreatePushInAppModel(NotificationTemplatePushInAppModel template, NotificationPushInAppModelFactoryOptions options)
    {
        var notificationText = new StringBuilder(template.Message.ToString());
        var notificationSubject = template.Subject.ToString();

        ApplyFormatting(ref notificationText);

        var pushModel = new NotificationPushInAppModel(notificationSubject, notificationText.ToString());
        return pushModel;
    }

    public NotificationSmsModel CreateSmsModel(NotificationTemplateSmsModel template, NotificationSmsModelFactoryOptions options)
    {
        var sms = new StringBuilder(template.Text.ToString());

        ApplyFormatting(ref sms);
        ApplyLinks(sms,
            smsLink =>
                $"\r\n{smsLink.Key}\r\n{options.WebsiteUrl}/{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}{smsLink.Value}");

        var smsModel = new NotificationSmsModel(sms.ToString());
        return smsModel;
    }

    public async Task<NotificationEmailModel> CreateEmailModelAsync(NotificationTemplateEmailModel template, NotificationEmailModelFactoryOptions options)
    {
        var emailHtml = new StringBuilder(template.Html.ToString());
        var emailShowRequestData = template.ShowRequestData;

        var htmlLayout = await NotificationTemplateEmailModel.GetLayout(_logger);
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
        //    node.ChildNodes[0].Attributes["alt"].Value = _localizer[socialMedia.Name];
        //    footerSocial.AppendChild(node);
        //}

        ApplyLinks(emailHtml,
            emailButton =>
                $"<p><a href='{options.WebsiteUrl}/{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}{emailButton.Value}' class='e-button'>{emailButton.Key}</a></p>");

        var requestHtml = await NotificationTemplateEmailModel.GetRequestLayoutAsync(_logger);
        if (string.IsNullOrEmpty(requestHtml))
            throw new Exception("Unable to find Request Html Layout.");

        ApplyFormatting(ref emailHtml);

        doc.QuerySelector(".js-content").InnerHtml = emailHtml.ToString();
        htmlLayout = doc.DocumentNode.OuterHtml;

        NotificationTemplateEmailModel.GetLayoutDictionary(dict, _notificationOptions.Receiver.Name, options, _localizer);
        if (emailShowRequestData)
            NotificationTemplateEmailModel.GetRequestDictionary(dict, options, _notificationOptions.RequestInfo, _localizer);

        NotificationModelFactoryExtensions.ApplyMinification(ref htmlLayout);

        return NotificationEmailModel.GetInstance(htmlLayout, dict);
    }
    private void ApplyLinks(StringBuilder emailHtml, Func<NotificationLink, string> s)
    {
        if (_notificationOptions.Content == null || !_notificationOptions.Content.Any())
            return;

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
        if (_notificationOptions.Content == null || !_notificationOptions.Content.Any())
            return;

        var formattings = _notificationOptions.Content.OfType<NotificationFormatting>();
        if (!formattings.Any())
            return;

        foreach (var formatting in formattings)
        {
            emailHtml = emailHtml.Replace("{{" + formatting.Key + "}}", formatting.Value);
        }
    }
    public NotificationPushModel CreatePushModel(NotificationTemplatePushModel template, NotificationPushModelFactoryOptions options)
    {
        var notificationText = new StringBuilder(template.Message.ToString());
        var notificationSubject = template.Subject.ToString();

        ApplyFormatting(ref notificationText);

        var pushModel = new NotificationPushModel(notificationSubject, notificationText.ToString());
        return pushModel;
    }
}