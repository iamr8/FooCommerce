using System.Text;

using FooCommerce.Domain.ContextRequest;
using FooCommerce.Domain.Enums;
using FooCommerce.Domain.Helpers;
using FooCommerce.Services.NotificationAPI.Attributes;
using FooCommerce.Services.NotificationAPI.Dtos;
using FooCommerce.Services.NotificationAPI.Enums;
using FooCommerce.Services.NotificationAPI.Exceptions;
using FooCommerce.Services.NotificationAPI.Handlers;
using FooCommerce.Services.NotificationAPI.Interfaces;

namespace FooCommerce.Services.NotificationAPI.Services.Repositories;

public class Coordinator : ICoordinator
{
    private readonly ITemplateService _templateService;

    private readonly IEnumerable<IHandler> _handlers;

    public Coordinator(ITemplateService templateService, IEnumerable<IHandler> handlers)
    {
        _templateService = templateService;
        _handlers = handlers;
    }

    public async Task EnqueueAsync(NotificationPurpose purpose,
        string receiverName,
        IDictionary<CommType, string> communications,
        IDictionary<string, string> links,
        IDictionary<string, string> formatters,
        string baseUrl,
        ContextRequestInfo requestInfo,
        CancellationToken cancellationToken = default)
    {
        if (receiverName == null)
            throw new ArgumentNullException(nameof(receiverName));
        if (communications == null)
            throw new ArgumentNullException(nameof(communications));
        if (links == null)
            throw new ArgumentNullException(nameof(links));
        if (formatters == null)
            throw new ArgumentNullException(nameof(formatters));
        if (baseUrl == null)
            throw new ArgumentNullException(nameof(baseUrl));
        if (requestInfo == null)
            throw new ArgumentNullException(nameof(requestInfo));
        if (communications.Count == 0)
            throw new ArgumentException("Value cannot be an empty collection.", nameof(communications));

        var availableCommunicationTypes = purpose
              .GetAttribute<CommTypeAttribute>()
              .CommunicationTypes;

        var notificationModel = await _templateService.GetNotificationModelAsync(purpose, cancellationToken);
        foreach (var type in availableCommunicationTypes)
        {
            ITemplate template = type switch
            {
                CommType.Email => notificationModel.Templates.OfType<EmailTemplateModel>().SingleOrDefault(),
                CommType.Sms => notificationModel.Templates.OfType<SmsTemplateModel>().SingleOrDefault(),
                CommType.Push => notificationModel.Templates.OfType<PushTemplateModel>().SingleOrDefault(),
                _ => throw new ArgumentOutOfRangeException()
            };
            if (template == null)
                throw new TemplateModelNotFoundException(type, purpose);

            IHandler handler = type switch
            {
                CommType.Email => _handlers.OfType<EmailHandler>().SingleOrDefault(),
                CommType.Sms => _handlers.OfType<SmsHandler>().SingleOrDefault(),
                CommType.Push => _handlers.OfType<PushHandler>().SingleOrDefault(),
                _ => throw new ArgumentOutOfRangeException()
            };
            if (handler == null)
                throw new HandlerNotFoundException(type);

            await handler.EnqueueAsync(template,
                purpose,
                receiverName,
                communications[type],
                links,
                formatters,
                baseUrl,
                requestInfo,
                cancellationToken);
        }
    }

    public static void ApplyLinks(IDictionary<string, string> links, StringBuilder emailHtml, Func<KeyValuePair<string, string>, string> s)
    {
        if (links == null || !links.Any())
            return;

        // for (int i = 0; i < links.Count; i++)
        // {
        //     var link = links.ElementAt(i);
        //     emailHtml.Replace(s(link), link.Value);
        // }
        foreach (var link in links)
        {
            emailHtml.Append(s(link));
        }
    }

    public static void ApplyFormatters(IDictionary<string, string> formatters, ref StringBuilder emailHtml)
    {
        if (formatters == null || !formatters.Any())
            return;

        foreach (var formatting in formatters)
        {
            emailHtml = emailHtml.Replace("{{" + formatting.Key + "}}", formatting.Value);
        }
    }
}