using System.Globalization;
using System.Text;

using FooCommerce.Domain.ContextRequest;
using FooCommerce.Localization;
using FooCommerce.Services.NotificationAPI.Contracts;
using FooCommerce.Services.NotificationAPI.Dtos;
using FooCommerce.Services.NotificationAPI.Enums;
using FooCommerce.Services.NotificationAPI.Interfaces;
using FooCommerce.Services.NotificationAPI.Models;
using FooCommerce.Services.NotificationAPI.Services.Repositories;

using MassTransit;

namespace FooCommerce.Services.NotificationAPI.Handlers;

public class SmsHandler : MessageHandler
{
    private readonly ILogger<EmailHandler> _logger;

    public SmsHandler(ILocalizer localizer, IBus bus, ILogger<EmailHandler> logger) : base(bus, localizer)
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
        if (template is not SmsTemplateModel smsTemplate)
            throw new ArgumentException("Template is not SMS template", nameof(template));

        var sms = new StringBuilder(smsTemplate.Text.ToString());

        Coordinator.ApplyFormatters(formatters, ref sms);
        Coordinator.ApplyLinks(links, sms, smsLink => $"\r\n{smsLink.Key}\r\n{websiteUrl}/{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}{smsLink.Value}");

        await Bus.Publish<EnqueueSms>(new
        {
            Body = sms.ToString(),
            ReceiverName = receiverName,
            ReceiverAddress = receiverAddress,
        }, cancellationToken);
    }
}