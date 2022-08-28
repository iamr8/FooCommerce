using FooCommerce.NotificationAPI.Dtos;
using FooCommerce.NotificationAPI.Models.FactoryOptions;
using FooCommerce.NotificationAPI.Models.Types;

namespace FooCommerce.NotificationAPI.Interfaces;

public interface INotificationModelFactory
{
    Task<NotificationEmailModel> CreateEmailModelAsync(NotificationTemplateEmailModel template,
        NotificationEmailModelFactoryOptions options);

    NotificationPushModel CreatePushModel(NotificationTemplatePushModel template,
        NotificationPushModelFactoryOptions options);

    NotificationPushInAppModel CreatePushInAppModel(NotificationTemplatePushInAppModel template,
        NotificationPushInAppModelFactoryOptions options);

    NotificationSmsModel CreateSmsModel(NotificationTemplateSmsModel template,
        NotificationSmsModelFactoryOptions options);
}