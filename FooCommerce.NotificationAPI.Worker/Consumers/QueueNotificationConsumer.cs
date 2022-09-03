using FooCommerce.Common.Helpers;
using FooCommerce.Common.Localization;
using FooCommerce.Domain.Enums;
using FooCommerce.NotificationAPI.Attributes;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Models;
using FooCommerce.NotificationAPI.Worker.Contracts;
using FooCommerce.NotificationAPI.Worker.Dtos;
using FooCommerce.NotificationAPI.Worker.Models;
using FooCommerce.NotificationAPI.Worker.Models.FactoryOptions;
using FooCommerce.NotificationAPI.Worker.Services;

using MassTransit;

namespace FooCommerce.NotificationAPI.Worker.Consumers;

public class QueueNotificationConsumer
    : IConsumer<QueueNotification>
{
    private readonly INotificationTemplateService _templateService;
    private readonly ILogger<QueueNotificationConsumer> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IConfiguration _configuration;
    private readonly ILocalizer _localizer;

    public QueueNotificationConsumer(INotificationTemplateService templateService,
        IConfiguration configuration,
        ILoggerFactory loggerFactory,
        ILocalizer localizer)
    {
        _templateService = templateService;
        _loggerFactory = loggerFactory;
        _localizer = localizer;
        _logger = _loggerFactory.CreateLogger<QueueNotificationConsumer>();
        _configuration = configuration;
    }

    public async Task Consume(ConsumeContext<QueueNotification> context)
    {
        var availableCommunicationTypes = context.Message.Action
               .GetAttribute<NotificationCommunicationTypeAttribute>()
               .CommunicationTypes;

        var websiteUrl = _configuration["WebsiteURL"];
        var notificationModel = await _templateService.GetNotificationModelAsync(context.Message.Action, context.CancellationToken);
        if (notificationModel == null || !notificationModel.Templates.Any())
            throw new NullReferenceException("Unable to find any corresponding templates.");

        var factory = NotificationModelFactory.CreateFactory(context.Message, _loggerFactory, _localizer);

        foreach (var communicationType in availableCommunicationTypes)
        {
            switch (communicationType)
            {
                case CommunicationType.Email_Message:
                    {
                        var template = notificationModel.Templates.OfType<NotificationTemplateEmailModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {0}, but unable to find appropriate {1} template for it.", communicationType, context.Message.Action);
                            continue;
                        }

                        var hasReceiver =
                            context.Message.ReceiverProvider.Communications.TryGetValue(
                                CommunicationType.Email_Message, out var receiver);
                        if (!hasReceiver)
                        {
                            _logger.LogError("Action {0} needs to send notification via {0}, but unable to find receiver's address.", communicationType);
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
                            Receiver = new NotificationReceiver { Name = context.Message.ReceiverProvider.Name, UserId = context.Message.ReceiverProvider.UserId, Address = receiver },
                            notificationModel.NotificationId,
                            context.Message.Action,
                            context.Message.RequestInfo,
                            context.Message.Bag,
                            context.Message.Formatters,
                            context.Message.Links,
                            context.Message.UserId
                        }, context.CancellationToken);
                        break;
                    }
                case CommunicationType.Push_Notification:
                    {
                        var template = notificationModel.Templates.OfType<NotificationTemplatePushModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {0}, but unable to find appropriate {1} template for it.", communicationType, context.Message.Action);
                            continue;
                        }

                        var hasReceiver =
                            context.Message.ReceiverProvider.Communications.TryGetValue(
                                CommunicationType.Email_Message, out var receiver);
                        if (!hasReceiver)
                        {
                            _logger.LogError("Action {0} needs to send notification via {0}, but unable to find receiver's address.", communicationType);
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
                            notificationModel.NotificationId,
                            context.Message.Action,
                            context.Message.RequestInfo,
                            Receiver = new NotificationReceiver { Name = context.Message.ReceiverProvider.Name, UserId = context.Message.ReceiverProvider.UserId, Address = receiver },
                            context.Message.Bag,
                            context.Message.Formatters,
                            context.Message.Links
                        }, context.CancellationToken);
                        break;
                    }
                case CommunicationType.Mobile_Sms:
                    {
                        var template = notificationModel.Templates.OfType<NotificationTemplateSmsModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {0}, but unable to find appropriate {1} template for it.", communicationType, context.Message.Action);
                            continue;
                        }

                        var hasReceiver =
                            context.Message.ReceiverProvider.Communications.TryGetValue(
                                CommunicationType.Email_Message, out var receiver);
                        if (!hasReceiver)
                        {
                            _logger.LogError("Action {0} needs to send notification via {0}, but unable to find receiver's address.", communicationType);
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
                            notificationModel.NotificationId,
                            context.Message.Action,
                            context.Message.RequestInfo,
                            Receiver = new NotificationReceiver { Name = context.Message.ReceiverProvider.Name, UserId = context.Message.ReceiverProvider.UserId, Address = receiver },
                            context.Message.Bag,
                            context.Message.Formatters,
                            context.Message.Links
                        }, context.CancellationToken);
                        break;
                    }
                case CommunicationType.Push_InApp:
                    {
                        var template = notificationModel.Templates.OfType<NotificationTemplatePushInAppModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {0}, but unable to find appropriate {1} template for it.", communicationType, context.Message.Action);
                            continue;
                        }

                        var hasReceiver =
                            context.Message.ReceiverProvider.Communications.TryGetValue(
                                CommunicationType.Email_Message, out var receiver);
                        if (!hasReceiver)
                        {
                            _logger.LogError("Action {0} needs to send notification via {0}, but unable to find receiver's address.", communicationType);
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
                            notificationModel.NotificationId,
                            context.Message.Action,
                            context.Message.RequestInfo,
                            Receiver = new NotificationReceiver { Name = context.Message.ReceiverProvider.Name, UserId = context.Message.ReceiverProvider.UserId, Address = receiver },
                            context.Message.Bag,
                            context.Message.Formatters,
                            context.Message.Links
                        }, context.CancellationToken);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}