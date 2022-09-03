using System.Data;
using System.Text.Json.Nodes;

using Dapper;

using EasyCaching.Core;

using FooCommerce.Common.Caching;
using FooCommerce.Common.Localization.Helpers;
using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.Enums;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Worker.DbProvider.Entities;
using FooCommerce.NotificationAPI.Worker.Dtos;
using FooCommerce.NotificationAPI.Worker.Interfaces;

using MassTransit;

namespace FooCommerce.NotificationAPI.Worker.Services.Repositories
{
    public class NotificationTemplateService : INotificationTemplateService
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IEasyCachingProvider _easyCaching;
        private readonly ILogger<INotificationTemplateService> _logger;

        public NotificationTemplateService(IDbConnectionFactory dbConnectionFactory,
            IEasyCachingProvider easyCaching,
            ILogger<INotificationTemplateService> logger)
        {
            _logger = logger;
            _dbConnectionFactory = dbConnectionFactory;
            _easyCaching = easyCaching;
        }

        public const string CacheKey = "config.notifications.templates";

        public async ValueTask<INotification> GetNotificationModelAsync(NotificationAction actionName, CancellationToken cancellationToken = default)
        {
            using var dbConnection = _dbConnectionFactory.CreateConnection();

            return await GetNotificationModelAsync(actionName, dbConnection, cancellationToken);
        }

        private async ValueTask<INotification> GetNotificationModelAsync(NotificationAction actionName, IDbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{CacheKey}.{actionName.ToString().ToLowerInvariant()}";
            return await _easyCaching.GetOrCreateAsync(cacheKey,
                async () => await GetNotificationModelNonCachedAsync(actionName, dbConnection, cancellationToken), _logger, cancellationToken);
        }

        private static NotificationTemplateSmsModel ParseMobileModel(NotificationTemplateModel template, JsonNode json)
        {
            var text = json["t"];

            var localizerText = LocalizerSerializationHelper.Deserialize(text);

            return new NotificationTemplateSmsModel(template.Id)
            {
                Text = localizerText
            };
        }

        private static NotificationTemplatePushModel ParsePushModel(NotificationTemplateModel template, JsonNode json)
        {
            var subject = json["s"];
            var message = json["m"];

            var localizerSubject = LocalizerSerializationHelper.Deserialize(subject);
            var localizerMessage = LocalizerSerializationHelper.Deserialize(message);

            return new NotificationTemplatePushModel(template.Id)
            {
                Subject = localizerSubject,
                Message = localizerMessage
            };
        }

        private static NotificationTemplateEmailModel ParseEmailModel(NotificationTemplateModel template, JsonNode json)
        {
            var html = json["h"];

            var localizerHtml = LocalizerSerializationHelper.Deserialize(html);

            return new NotificationTemplateEmailModel(template.Id)
            {
                ShowRequestData = template.IncludeRequest,
                Html = localizerHtml
            };
        }

        private static async Task<INotification> GetNotificationModelNonCachedAsync(NotificationAction actionName, IDbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException(cancellationToken);

                var notificationId = (await dbConnection.QuerySingleOrDefaultAsync<Guid?>(
                    $"SELECT TOP(1) [notification].{nameof(Notification.Id)} " +
                    $"FROM [Notifications] AS [notification] " +
                    $"WHERE [notification].{nameof(Notification.Action)} = @Action " +
                    $"ORDER BY [notification].{nameof(Notification.Created)} DESC",
                    new
                    {
                        Action = (short)actionName
                    }));
                if (notificationId == null || notificationId == Guid.Empty)
                    throw new NullReferenceException($"Unable to find a template related to the '{actionName}' action.");

                // Must be grouped by gateway
                var __templates = await dbConnection.QueryAsync<NotificationTemplateModel>(
                    $"SELECT [template].{nameof(NotificationTemplate.Id)}, [template].{nameof(NotificationTemplate.Type)}, [template].{nameof(NotificationTemplate.JsonTemplate)}, [template].{nameof(NotificationTemplate.IncludeRequest)} " +
                    $"FROM [NotificationTemplates] AS [template] " +
                    $"WHERE [template].{nameof(NotificationTemplate.NotificationId)} = @NotificationId " +
                    $"ORDER BY [template].{nameof(NotificationTemplate.Created)}",
                    new
                    {
                        NotificationId = notificationId.Value
                    });
                if (!__templates.Any())
                    throw new NullReferenceException($"Unable to find a template related to the '{actionName}' action.");

                var templates = __templates.AsList();
                var outputTemplates = new List<INotificationTemplate>();
                foreach (var template in templates)
                {
                    var json = JsonNode.Parse(template.JsonTemplate);

                    switch (template.Type)
                    {
                        case CommunicationType.Email_Message:
                            {
                                outputTemplates.Add(ParseEmailModel(template, json));
                                break;
                            }
                        case CommunicationType.Push_InApp:
                        case CommunicationType.Push_Notification:
                            {
                                outputTemplates.Add(ParsePushModel(template, json));
                                break;
                            }

                        case CommunicationType.Mobile_Sms:
                            {
                                outputTemplates.Add(ParseMobileModel(template, json));
                                break;
                            }

                        default:
                            throw new ArgumentOutOfRangeException(nameof(template.Type));
                    }
                }

                var output = new NotificationModel
                {
                    NotificationId = notificationId.Value,
                    Templates = outputTemplates
                };

                return output;
            }
        }
    }
}