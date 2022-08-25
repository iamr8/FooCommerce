using FooCommerce.Application.Notifications.Enums;
using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.Application.Notifications.Services;

public interface INotificationTemplateService
{
    ValueTask<IEnumerable<INotificationTemplate>> GetTemplateAsync(NotificationAction actionName, CancellationToken cancellationToken = default);
}