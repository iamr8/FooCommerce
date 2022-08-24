using FooCommerce.Application.Attributes.Communications;
using FooCommerce.Application.Commands.Notifications;
using FooCommerce.Application.Dtos.Notifications;
using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Helpers;
using FooCommerce.Application.Models.Notifications.Options;
using FooCommerce.Application.Services.Notifications;
using FooCommerce.NotificationAPI.Models;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Commands;

public class SendNotificationHandler : INotificationHandler<SendNotification>
{
    private readonly INotificationTemplateService _notificationTemplateService;
    private readonly ILogger<SendNotificationHandler> _logger;
    private readonly IMediator _mediator;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IConfiguration _configuration;

    public SendNotificationHandler(INotificationTemplateService notificationTemplateService,
        IMediator mediator,
        IConfiguration configuration,
        ILoggerFactory loggerFactory)
    {
        _notificationTemplateService = notificationTemplateService;
        _mediator = mediator;
        _loggerFactory = loggerFactory;
        _logger = _loggerFactory.CreateLogger<SendNotificationHandler>();
        _configuration = configuration;
    }

    public async Task Handle(SendNotification notification, CancellationToken cancellationToken)
    {
        var availableCommunicationTypes = notification.Options.Action
                .GetAttribute<NotificationCommunicationTypeAttribute>()
                .CommunicationTypes;

        await using var mailClient = await _mediator.Send(new GetAvailableMailClient(), cancellationToken);

        var websiteUrl = _configuration["WebsiteURL"];
        var templates = await _notificationTemplateService.GetTemplateAsync(notification.Options.Action, cancellationToken);
        var factory = NotificationModelFactory.CreateFactory(notification.Options, _loggerFactory);

        foreach (var communicationType in availableCommunicationTypes)
        {
            switch (communicationType)
            {
                case CommunicationType.Email_Message:
                    {
                        var template = templates.OfType<NotificationTemplateEmailModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {1}, but unable to find appropriate {1} template for it.", notification.Options.Action, communicationType);
                            continue;
                        }

                        await _mediator.Publish(new SendNotificationEmail(new SendNotificationEmailOptions
                        {
                            Options = notification.Options,
                            WebsiteUrl = websiteUrl,
                            Template = template,
                            Factory = factory,
                            RequestInfo = notification.Options.RequestInfo,
                            IsImportant = true
                        }), cancellationToken);
                        break;
                    }
                case CommunicationType.Push_Notification:
                    {
                        var template = templates.OfType<NotificationTemplatePushModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {1}, but unable to find appropriate {1} template for it.", notification.Options.Action, communicationType);
                            continue;
                        }

                        await _mediator.Publish(new SendNotificationPush(new SendNotificationPushOptions
                        {
                            Options = notification.Options,
                            WebsiteUrl = websiteUrl,
                            Template = template,
                            Factory = factory,
                            RequestInfo = notification.Options.RequestInfo,
                        }), cancellationToken);
                        break;
                    }
                case CommunicationType.Mobile_Sms:
                    {
                        var template = templates.OfType<NotificationTemplateSmsModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {1}, but unable to find appropriate {1} template for it.", notification.Options.Action, communicationType);
                            continue;
                        }

                        await _mediator.Publish(new SendNotificationSms(new SendNotificationSmsOptions
                        {
                            Options = notification.Options,
                            WebsiteUrl = websiteUrl,
                            Template = template,
                            Factory = factory,
                            RequestInfo = notification.Options.RequestInfo,
                        }), cancellationToken);
                        break;
                    }
                case CommunicationType.Push_InApp:
                    {
                        var template = templates.OfType<NotificationTemplatePushInAppModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {1}, but unable to find appropriate {1} template for it.", notification.Options.Action, communicationType);
                            continue;
                        }

                        await _mediator.Publish(new SendNotificationPushInApp(new SendNotificationPushInAppOptions
                        {
                            Options = notification.Options,
                            WebsiteUrl = websiteUrl,
                            Template = template,
                            Factory = factory,
                            RequestInfo = notification.Options.RequestInfo,
                        }), cancellationToken);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}