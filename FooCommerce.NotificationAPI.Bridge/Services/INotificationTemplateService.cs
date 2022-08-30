using FooCommerce.NotificationAPI.Bridge.Enums;
using FooCommerce.NotificationAPI.Bridge.Interfaces;

namespace FooCommerce.NotificationAPI.Bridge.Services;

public interface INotificationTemplateService
{
    ValueTask<IEnumerable<INotificationTemplate>> GetTemplateAsync(NotificationAction actionName, CancellationToken cancellationToken = default);
}