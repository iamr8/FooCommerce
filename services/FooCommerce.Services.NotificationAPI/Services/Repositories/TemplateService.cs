using System.Text.Json.Nodes;

using FooCommerce.Domain.Enums;
using FooCommerce.Localization.Helpers;
using FooCommerce.Services.NotificationAPI.DbProvider;
using FooCommerce.Services.NotificationAPI.Dtos;
using FooCommerce.Services.NotificationAPI.Enums;
using FooCommerce.Services.NotificationAPI.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Services.NotificationAPI.Services.Repositories;

public class TemplateService : ITemplateService
{
    private readonly IDbContextFactory<NotificationDbContext> _dbContextFactory;

    public TemplateService(IDbContextFactory<NotificationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<INotification> GetNotificationModelAsync(NotificationPurpose purposeName, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var notificationId = await dbContext.Notifications
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Action == (short)purposeName)
            .OrderByDescending(x => x.Created)
            .Select(x => new { x.Id })
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (notificationId == null || notificationId.Id == Guid.Empty)
            throw new NullReferenceException($"Unable to find a template related to the '{purposeName}' action.");

        // Must be grouped by gateway
        var __templates = await dbContext.NotificationTemplates
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.NotificationId == notificationId.Id)
            .OrderBy(x => x.Created)
            .Select(x => new
            {
                x.Id,
                x.Type,
                x.JsonTemplate,
                x.IncludeRequest
            })
            .ToListAsync(cancellationToken: cancellationToken);
        if (__templates == null || !__templates.Any())
            throw new NullReferenceException($"Unable to find a template related to the '{purposeName}' action.");

        var temps = __templates.Select(x => new NotificationTemplateModel
        {
            IncludeRequest = x.IncludeRequest,
            JsonTemplate = x.JsonTemplate,
            Type = x.Type,
            Id = x.Id
        });
        var outputTemplates = new List<ITemplate>();
        foreach (var template in temps)
        {
            switch (template.Type)
            {
                case CommType.Email:
                    {
                        outputTemplates.Add(ParseEmailModel(template));
                        break;
                    }
                case CommType.Push:
                    {
                        outputTemplates.Add(ParsePushModel(template));
                        break;
                    }

                case CommType.Sms:
                    {
                        outputTemplates.Add(ParseMobileModel(template));
                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(template.Type));
            }
        }

        var output = new NotificationModel
        {
            NotificationId = notificationId.Id,
            Templates = outputTemplates
        };

        return output;
    }

    private static SmsTemplateModel ParseMobileModel(NotificationTemplateModel template)
    {
        var json = JsonNode.Parse(template.JsonTemplate);
        var text = json["t"];

        var localizerText = LocalizerSerializationHelper.Deserialize(text);

        return new SmsTemplateModel
        {
            Text = localizerText
        };
    }

    private static PushTemplateModel ParsePushModel(NotificationTemplateModel template)
    {
        var json = JsonNode.Parse(template.JsonTemplate);
        var subject = json["s"];
        var message = json["m"];

        var localizerSubject = LocalizerSerializationHelper.Deserialize(subject);
        var localizerMessage = LocalizerSerializationHelper.Deserialize(message);

        return new PushTemplateModel
        {
            Subject = localizerSubject,
            Message = localizerMessage
        };
    }

    private static EmailTemplateModel ParseEmailModel(NotificationTemplateModel template)
    {
        var json = JsonNode.Parse(template.JsonTemplate);
        var html = json["h"];

        var localizerHtml = LocalizerSerializationHelper.Deserialize(html);

        return new EmailTemplateModel
        {
            ShowRequestData = template.IncludeRequest,
            Html = localizerHtml
        };
    }
}