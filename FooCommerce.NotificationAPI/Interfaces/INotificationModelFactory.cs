using FooCommerce.Application.Notifications.Models.Types;
using FooCommerce.NotificationAPI.Dtos;
using FooCommerce.NotificationAPI.Models.FactoryOptions;

namespace FooCommerce.NotificationAPI.Interfaces;

public interface INotificationModelFactory
{
    Task<NotificationEmailModel> CreateEmailModelAsync(NotificationTemplateEmailModel template, Action<NotificationEmailModelFactoryOptions> options);

    NotificationPushModel CreatePushModel(NotificationTemplatePushModel template, Action<NotificationPushModelFactoryOptions> options);

    NotificationPushInAppModel CreatePushInAppModel(NotificationTemplatePushInAppModel template, Action<NotificationPushInAppModelFactoryOptions> options);

    NotificationSmsModel CreateSmsModel(NotificationTemplateSmsModel template, Action<NotificationSmsModelFactoryOptions> options);
}