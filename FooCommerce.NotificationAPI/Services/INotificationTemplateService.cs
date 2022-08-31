using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Services;

public interface INotificationTemplateService
{
    ValueTask<IEnumerable<INotificationTemplate>> GetTemplateAsync(NotificationAction actionName, CancellationToken cancellationToken = default);
}