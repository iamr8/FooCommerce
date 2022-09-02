using System.Data;
using System.Text.Json.Nodes;

using Dapper;

using EasyCaching.Core;

using FooCommerce.Common.Caching;
using FooCommerce.Common.Localization.Helpers;
using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.Enums;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Worker.DbProvider.Entities;
using FooCommerce.NotificationAPI.Worker.Dtos;

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

        public async ValueTask<IEnumerable<INotificationTemplate>> GetTemplateAsync(NotificationAction actionName, CancellationToken cancellationToken = default)
        {
            using var dbConnection = _dbConnectionFactory.CreateConnection();

            return await GetTemplateAsync(actionName, dbConnection, cancellationToken);
        }

        private async ValueTask<IEnumerable<INotificationTemplate>> GetTemplateAsync(NotificationAction actionName, IDbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{CacheKey}.{actionName.ToString().ToLowerInvariant()}";
            return await _easyCaching.GetOrCreateAsync(cacheKey,
                async () => await GetTemplateNonCachedAsync(actionName, dbConnection, cancellationToken), _logger, cancellationToken);
        }

        private static async Task<IEnumerable<INotificationTemplate>> GetTemplateNonCachedAsync(NotificationAction actionName, IDbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException(cancellationToken);

                var templates = new List<NotificationTemplateModel>();
                var notificationIds = (await dbConnection.QueryAsync<Guid?>(
                    $"SELECT TOP(1) [notification].{nameof(Notification.Id)} " +
                    $"FROM [Notifications] AS [notification] " +
                    $"WHERE [notification].{nameof(Notification.Action)} = @Action " +
                    $"ORDER BY [notification].{nameof(Notification.Created)} DESC",
                    new
                    {
                        Action = (short)actionName
                    })).AsList();
                if (notificationIds == null || !notificationIds.Any())
                    throw new NullReferenceException($"Unable to find a template related to the '{actionName}' action.");

                var notificationId = notificationIds[0];
                if (notificationId is not { })
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

                templates = __templates.AsList();
                var output = new List<INotificationTemplate>();
                for (var i = 0; i < templates.Count; i++)
                {
                    var template = templates[i];
                    var json = JsonNode.Parse(template.JsonTemplate);

                    switch (template.Type)
                    {
                        case CommunicationType.Email_Message:
                            {
                                var html = json["h"];

                                var localizerHtml = LocalizerSerializationHelper.Deserialize(html);

                                output.Add(new NotificationTemplateEmailModel(template.Id)
                                {
                                    ShowRequestData = template.IncludeRequest,
                                    Html = localizerHtml
                                });
                                break;
                            }
                        case CommunicationType.Push_Notification:
                            {
                                var subject = json["s"];
                                var message = json["m"];

                                var localizerSubject = LocalizerSerializationHelper.Deserialize(subject);
                                var localizerMessage = LocalizerSerializationHelper.Deserialize(message);

                                output.Add(new NotificationTemplatePushModel(template.Id)
                                {
                                    Subject = localizerSubject,
                                    Message = localizerMessage
                                });
                                break;
                            }

                        case CommunicationType.Mobile_Sms:
                            {
                                var text = json["t"];

                                var localizerText = LocalizerSerializationHelper.Deserialize(text);

                                output.Add(new NotificationTemplateSmsModel(template.Id)
                                {
                                    Text = localizerText
                                });
                                break;
                            }

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                return output;
            }
        }
    }
}