using System.Data;

using Dapper;

using EasyCaching.Core;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Dtos.Notifications;
using FooCommerce.Application.Entities.Messagings;
using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Enums.Notifications;
using FooCommerce.Application.Interfaces.Notifications;
using FooCommerce.Application.Services.Notifications;
using FooCommerce.Infrastructure.Caching;
using FooCommerce.Infrastructure.Localization.Helpers;
using FooCommerce.NotificationAPI.Dtos;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

namespace FooCommerce.NotificationAPI.Services
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
                async () => await GetTemplateNonCachedAsync(actionName, dbConnection), _logger, cancellationToken);
        }

        private static async Task<IEnumerable<INotificationTemplate>> GetTemplateNonCachedAsync(NotificationAction actionName, IDbConnection dbConnection)
        {
            const string notificationQuery = $"SELECT TOP(1) [notification].{nameof(Notification.Id)} " +
                                             $"FROM [Notifications] AS [notification] " +
                                             $"WHERE [notification].{nameof(Notification.Action)} = @Action" +
                                             $"ORDER BY [notification].{nameof(Notification.Created)} DESC";
            var notificationId = await dbConnection.QueryFirstAsync<Guid?>(notificationQuery,
                new
                {
                    Action = (short)actionName
                });
            if (notificationId is not { })
                throw new NullReferenceException($"Unable to find a template related to the '{actionName}' action.");

            const string templatesQuery = $"SELECT [template].{nameof(NotificationTemplate.Id)}, [notification].{nameof(NotificationTemplate.Type)}, [notification].{nameof(NotificationTemplate.JsonTemplate)}, [notification].{nameof(NotificationTemplate.IncludeRequest)} " +
                                          $"FROM [NotificationTemplates] AS [template] " +
                                          $"WHERE [template].{nameof(NotificationTemplate.NotificationId)} = @NotificationId" +
                                          $"ORDER BY [template].{nameof(NotificationTemplate.Created)}";
            // Must be grouped by gateway
            var __templates = await dbConnection.QueryAsync<NotificationTemplateModel>(templatesQuery,
                new
                {
                    NotificationId = notificationId.Value
                });
            if (!__templates.Any())
                throw new NullReferenceException($"Unable to find a template related to the '{actionName.ToString()}' action.");

            var _templates = __templates.AsList();
            var output = new List<INotificationTemplate>();
            for (var i = 0; i < _templates.Count; i++)
            {
                var template = _templates[i];
                var json = JObject.Parse(template.JsonTemplate);

                switch (template.Type)
                {
                    case CommunicationType.Email_Message:
                        {
                            var html = json["h"].ToString();

                            var localizerHtml = LocalizerHelper.Deserialize(html);

                            output.Add(new NotificationTemplateEmailModel(template.Id)
                            {
                                Id = template.Id,
                                ShowRequestData = template.IncludeRequest,
                                Html = localizerHtml
                            });
                            break;
                        }
                    case CommunicationType.Push_Notification:
                        {
                            var subject = json["s"].ToString();
                            var message = json["m"].ToString();

                            var localizerSubject = LocalizerHelper.Deserialize(subject);
                            var localizerMessage = LocalizerHelper.Deserialize(message);

                            output.Add(new NotificationTemplatePushModel(template.Id)
                            {
                                Id = template.Id,
                                Subject = localizerSubject,
                                Message = localizerMessage
                            });
                            break;
                        }

                    case CommunicationType.Mobile_Sms:
                        {
                            var text = json["t"].ToString();

                            var localizerText = LocalizerHelper.Deserialize(text);

                            output.Add(new NotificationTemplateSmsModel(template.Id)
                            {
                                Id = template.Id,
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