﻿using FooCommerce.Application.Communications.Enums;
using FooCommerce.Application.Helpers;
using FooCommerce.Core.DbProvider;
using FooCommerce.Core.Helpers;
using FooCommerce.Domain;
using FooCommerce.NotificationAPI.Attributes;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Services;
using FooCommerce.NotificationAPI.Worker.Contracts;
using FooCommerce.NotificationAPI.Worker.Dtos;
using FooCommerce.NotificationAPI.Worker.Models;
using FooCommerce.NotificationAPI.Worker.Models.FactoryOptions;

using MassTransit;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.NotificationAPI.Worker.Consumers;

public class QueueNotificationConsumer
    : IConsumer<QueueNotification>
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly INotificationTemplateService _templateService;
    private readonly ILogger<QueueNotificationConsumer> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IConfiguration _configuration;
    private readonly ILocalizer _localizer;

    public QueueNotificationConsumer(INotificationTemplateService templateService,
        IDbContextFactory<AppDbContext> dbContextFactory,
        IConfiguration configuration,
        ILoggerFactory loggerFactory, ILocalizer localizer)
    {
        _templateService = templateService;
        _loggerFactory = loggerFactory;
        _localizer = localizer;
        _dbContextFactory = dbContextFactory;
        _logger = _loggerFactory.CreateLogger<QueueNotificationConsumer>();
        _configuration = configuration;
    }

    public async Task Consume(ConsumeContext<QueueNotification> context)
    {
        var availableCommunicationTypes = context.Message.Action
               .GetAttribute<NotificationCommunicationTypeAttribute>()
               .CommunicationTypes;

        var websiteUrl = _configuration["WebsiteURL"];
        var templates = await _templateService.GetTemplateAsync(context.Message.Action, context.CancellationToken);
        if (templates == null || !templates.Any())
            throw new NullReferenceException("Unable to find any corresponding templates.");

        var factory = NotificationModelFactory.CreateFactory(context.Message, _loggerFactory, _localizer);

        // await context.Message.Receiver.ResolveInformationAsync(_dbContextFactory, context.CancellationToken);

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

                        var model = await factory.CreateEmailModelAsync(
                            template,
                            new NotificationEmailModelFactoryOptions
                            {
                                LocalDateTime = DateTime.UtcNow.ToLocal(context.Message.RequestInfo),
                                WebsiteUrl = websiteUrl,
                            });

                        await context.Publish<QueueNotificationEmail>(new
                        {
                            Model = model,
                            IsImportant = true,
                            context.Message.NotificationId,
                            context.Message.Action,
                            context.Message.RequestInfo,
                            context.Message.Receiver,
                            context.Message.Bag,
                            context.Message.Content
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

                        var model = factory.CreatePushModel(
                            template,
                            new NotificationPushModelFactoryOptions
                            {
                                WebsiteUrl = websiteUrl
                            });

                        await context.Publish<QueueNotificationPush>(new
                        {
                            Model = model,
                            context.Message.NotificationId,
                            context.Message.Action,
                            context.Message.RequestInfo,
                            context.Message.Receiver,
                            context.Message.Bag,
                            context.Message.Content
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

                        var model = factory.CreateSmsModel(
                            template,
                            new NotificationSmsModelFactoryOptions
                            {
                                WebsiteUrl = websiteUrl
                            });

                        await context.Publish<QueueNotificationSms>(new
                        {
                            Model = model,
                            context.Message.NotificationId,
                            context.Message.Action,
                            context.Message.RequestInfo,
                            context.Message.Receiver,
                            context.Message.Bag,
                            context.Message.Content
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

                        var model = factory.CreatePushInAppModel(
                            template,
                            new NotificationPushInAppModelFactoryOptions
                            {
                                WebsiteUrl = websiteUrl
                            });

                        await context.Publish<QueueNotificationPushInApp>(new
                        {
                            Model = model,
                            context.Message.NotificationId,
                            context.Message.Action,
                            context.Message.RequestInfo,
                            context.Message.Receiver,
                            context.Message.Bag,
                            context.Message.Content
                        }, context.CancellationToken);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}