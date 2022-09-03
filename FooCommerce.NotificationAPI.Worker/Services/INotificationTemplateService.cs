using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Worker.Interfaces;

namespace FooCommerce.NotificationAPI.Worker.Services;

public interface INotificationTemplateService
{
    ValueTask<INotification> GetNotificationModelAsync(NotificationAction actionName, CancellationToken cancellationToken = default);
}