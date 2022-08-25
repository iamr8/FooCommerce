using FooCommerce.Application.Notifications.Dtos;
using FooCommerce.Application.Notifications.Models.Options;
using FooCommerce.Application.Notifications.Models.Types;

namespace FooCommerce.Application.Notifications.Interfaces;

public interface INotificationModelFactory
{
    Task<NotificationEmailModel> CreateEmailModelAsync(NotificationTemplateEmailModel template, Action<NotificationEmailModelOptions> options);

    NotificationPushModel CreatePushModel(NotificationTemplatePushModel template, Action<NotificationPushModelOptions> options);

    NotificationPushInAppModel CreatePushInAppModel(NotificationTemplatePushInAppModel template, Action<NotificationPushInAppModelOptions> options);

    NotificationSmsModel CreateSmsModel(NotificationTemplateSmsModel template, Action<NotificationSmsModelOptions> options);
}