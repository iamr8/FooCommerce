using FooCommerce.Application.Enums.Notifications;
using FooCommerce.Application.Interfaces.Notifications;

namespace FooCommerce.Application.Services.Notifications;

public interface INotificationTemplateService
{
    ValueTask<IEnumerable<INotificationTemplate>> GetTemplateAsync(NotificationAction actionName, CancellationToken cancellationToken = default);
}