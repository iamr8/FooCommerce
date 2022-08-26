using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Helpers;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Notifications.Attributes;
using FooCommerce.Application.Notifications.Contracts;
using FooCommerce.Application.Notifications.Interfaces;
using FooCommerce.Application.Notifications.Services;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Dtos;
using FooCommerce.NotificationAPI.Events;
using FooCommerce.NotificationAPI.Models;

using MassTransit;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Consumers;

public class QueueNotificationConsumer
    : IConsumer<QueueNotification>
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly INotificationTemplateService _templateService;
    private readonly ILogger<QueueNotificationConsumer> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IConfiguration _configuration;

    public QueueNotificationConsumer(INotificationTemplateService templateService,
        IDbContextFactory<AppDbContext> dbContextFactory,
        IConfiguration configuration,
        ILoggerFactory loggerFactory)
    {
        _templateService = templateService;
        _loggerFactory = loggerFactory;
        _dbContextFactory = dbContextFactory;
        _logger = _loggerFactory.CreateLogger<QueueNotificationConsumer>();
        _configuration = configuration;
    }

    public QueueNotificationConsumer()
    {
    }

    public async Task Consume(ConsumeContext<QueueNotification> context)
    {
        if (context.Message.NotificationId == Guid.Empty)
        {
            await context.RespondAsync<NotificationFailed>(new
            {
                NotificationId = context.Message.NotificationId,
            });
            return;
        }

        var availableCommunicationTypes = context.Message.Action
               .GetAttribute<NotificationCommunicationTypeAttribute>()
               .CommunicationTypes;

        var websiteUrl = _configuration["WebsiteURL"];
        var templates = await _templateService.GetTemplateAsync(context.Message.Action, context.CancellationToken);
        var factory = NotificationModelFactory.CreateFactory(context.Message, _loggerFactory);

        await ((INotificationDataReceiver)context.Message.Receiver).ResolveReceiverInformationAsync(_dbContextFactory, context.CancellationToken);

        foreach (var communicationType in availableCommunicationTypes)
        {
            switch (communicationType)
            {
                case CommunicationType.Email_Message:
                    {
                        var template = templates.OfType<NotificationTemplateEmailModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {1}, but unable to find appropriate {1} template for it.", context.Message.Action, communicationType);
                            continue;
                        }

                        await context.Publish<QueueNotificationEmail>(new
                        {
                            NotificationId = context.Message.NotificationId,
                            Options = (INotificationOptions)context.Message,
                            WebsiteUrl = websiteUrl,
                            Template = (INotificationTemplate)template,
                            Factory = factory,
                            RequestInfo = context.Message.RequestInfo,
                            IsImportant = true
                        }, context.CancellationToken);
                        break;
                    }
                case CommunicationType.Push_Notification:
                    {
                        var template = templates.OfType<NotificationTemplatePushModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {1}, but unable to find appropriate {1} template for it.", context.Message.Action, communicationType);
                            continue;
                        }

                        await context.Publish<QueueNotificationPush>(new
                        {
                            NotificationId = context.Message.NotificationId,
                            Options = (INotificationOptions)context.Message,
                            WebsiteUrl = websiteUrl,
                            Template = (INotificationTemplate)template,
                            Factory = factory,
                            RequestInfo = context.Message.RequestInfo,
                        }, context.CancellationToken);
                        break;
                    }
                case CommunicationType.Mobile_Sms:
                    {
                        var template = templates.OfType<NotificationTemplateSmsModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {1}, but unable to find appropriate {1} template for it.", context.Message.Action, communicationType);
                            continue;
                        }

                        await context.Publish<QueueNotificationSms>(new
                        {
                            NotificationId = context.Message.NotificationId,
                            Options = (INotificationOptions)context.Message,
                            WebsiteUrl = websiteUrl,
                            Template = (INotificationTemplate)template,
                            Factory = factory,
                            RequestInfo = context.Message.RequestInfo,
                        }, context.CancellationToken);
                        break;
                    }
                case CommunicationType.Push_InApp:
                    {
                        var template = templates.OfType<NotificationTemplatePushInAppModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {1}, but unable to find appropriate {1} template for it.", context.Message.Action, communicationType);
                            continue;
                        }

                        await context.Publish<QueueNotificationPushInApp>(new
                        {
                            NotificationId = context.Message.NotificationId,
                            Options = (INotificationOptions)context.Message,
                            WebsiteUrl = websiteUrl,
                            Template = (INotificationTemplate)template,
                            Factory = factory,
                            RequestInfo = context.Message.RequestInfo,
                        }, context.CancellationToken);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        await context.RespondAsync<NotificationQueued>(new

        {
            NotificationId = context.Message.NotificationId
        });
    }
}