using FooCommerce.Application.Enums.Notifications;
using FooCommerce.Application.Interfaces.Notifications;

namespace FooCommerce.NotificationAPI.Services;

public interface INotificationTemplateService
{
    ValueTask<IEnumerable<INotificationTemplate>> GetTemplateAsync(NotificationAction actionName, CancellationToken cancellationToken = default);
}