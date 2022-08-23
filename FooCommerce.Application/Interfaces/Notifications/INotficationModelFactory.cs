using FooCommerce.Application.Dtos.Notifications;
using FooCommerce.Application.Models.Notifications.Options;
using FooCommerce.Application.Models.Notifications.Types;

namespace FooCommerce.Application.Interfaces.Notifications;

public interface INotificationModelFactory
{
    Task<NotificationEmailModel> CreateEmailModelAsync(NotificationTemplateEmailModel template, Action<NotificationEmailModelOptions> options);

    NotificationPushModel CreatePushModel(NotificationTemplatePushModel template, Action<NotificationPushModelOptions> options);

    NotificationPushInAppModel CreatePushInAppModel(NotificationTemplatePushInAppModel template, Action<NotificationPushInAppModelOptions> options);

    NotificationSmsModel CreateSmsModel(NotificationTemplateSmsModel template, Action<NotificationSmsModelOptions> options);
}