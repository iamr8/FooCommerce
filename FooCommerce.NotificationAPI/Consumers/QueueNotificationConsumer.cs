using FooCommerce.Application.Attributes.Communications;
using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Dtos.Notifications;
using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Helpers;
using FooCommerce.Application.Interfaces.Notifications;
using FooCommerce.Application.Models.Notifications.Options;
using FooCommerce.Application.Services.Notifications;
using FooCommerce.Infrastructure.JsonCustomization;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Models;
using FooCommerce.NotificationAPI.Publishers;

using MassTransit;
using MassTransit.Serialization;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Consumers;

public class QueueNotificationConsumer : IConsumer<QueueNotification>
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

    public async Task Consume(ConsumeContext<QueueNotification> context)
    {
        var availableCommunicationTypes = context.Message.Options.Action
               .GetAttribute<NotificationCommunicationTypeAttribute>()
               .CommunicationTypes;

        var websiteUrl = _configuration["WebsiteURL"];
        var templates = await _templateService.GetTemplateAsync(context.Message.Options.Action, context.CancellationToken);
        var factory = NotificationModelFactory.CreateFactory(context.Message.Options, _loggerFactory);

        await ((INotificationDataReceiver)context.Message.Options.Receiver).ResolveReceiverInformationAsync(_dbContextFactory, context.CancellationToken);

        foreach (var communicationType in availableCommunicationTypes)
        {
            switch (communicationType)
            {
                case CommunicationType.Email_Message:
                    {
                        var template = templates.OfType<NotificationTemplateEmailModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {1}, but unable to find appropriate {1} template for it.", context.Message.Options.Action, communicationType);
                            continue;
                        }

                        await context.Publish(new SendNotificationEmail(new SendNotificationEmailOptions
                        {
                            Options = context.Message.Options,
                            WebsiteUrl = websiteUrl,
                            Template = template,
                            Factory = factory,
                            RequestInfo = context.Message.Options.RequestInfo,
                            IsImportant = true
                        }), context.CancellationToken);
                        break;
                    }
                case CommunicationType.Push_Notification:
                    {
                        var template = templates.OfType<NotificationTemplatePushModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {1}, but unable to find appropriate {1} template for it.", context.Message.Options.Action, communicationType);
                            continue;
                        }

                        await context.Publish(new SendNotificationPush(new SendNotificationPushOptions
                        {
                            Options = context.Message.Options,
                            WebsiteUrl = websiteUrl,
                            Template = template,
                            Factory = factory,
                            RequestInfo = context.Message.Options.RequestInfo,
                        }), context.CancellationToken);
                        break;
                    }
                case CommunicationType.Mobile_Sms:
                    {
                        var template = templates.OfType<NotificationTemplateSmsModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {1}, but unable to find appropriate {1} template for it.", context.Message.Options.Action, communicationType);
                            continue;
                        }

                        await context.Publish(new SendNotificationSms(new SendNotificationSmsOptions
                        {
                            Options = context.Message.Options,
                            WebsiteUrl = websiteUrl,
                            Template = template,
                            Factory = factory,
                            RequestInfo = context.Message.Options.RequestInfo,
                        }), context.CancellationToken);
                        break;
                    }
                case CommunicationType.Push_InApp:
                    {
                        var template = templates.OfType<NotificationTemplatePushInAppModel>().SingleOrDefault();
                        if (template == null)
                        {
                            _logger.LogError("Action {0} needs to send notification via {1}, but unable to find appropriate {1} template for it.", context.Message.Options.Action, communicationType);
                            continue;
                        }

                        await context.Publish(new SendNotificationPushInApp(new SendNotificationPushInAppOptions
                        {
                            Options = context.Message.Options,
                            WebsiteUrl = websiteUrl,
                            Template = template,
                            Factory = factory,
                            RequestInfo = context.Message.Options.RequestInfo,
                        }), context.CancellationToken);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

public class QueueNotificationConsumerDefinition
    : ConsumerDefinition<QueueNotificationConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<QueueNotificationConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
        endpointConfigurator.ClearSerialization();
        endpointConfigurator.UseNewtonsoftJsonSerializer();
        endpointConfigurator.UseNewtonsoftJsonDeserializer();

        NewtonsoftJsonMessageSerializer.SerializerSettings = DefaultSettings.Settings;
        NewtonsoftJsonMessageSerializer.DeserializerSettings = DefaultSettings.Settings;
    }
}