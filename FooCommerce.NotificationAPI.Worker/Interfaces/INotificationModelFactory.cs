using FooCommerce.NotificationAPI.Worker.Dtos;
using FooCommerce.NotificationAPI.Worker.Models.Communications;
using FooCommerce.NotificationAPI.Worker.Models.FactoryOptions;

namespace FooCommerce.NotificationAPI.Worker.Interfaces;

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