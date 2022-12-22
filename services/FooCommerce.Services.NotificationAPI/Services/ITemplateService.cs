using FooCommerce.Services.NotificationAPI.Enums;
using FooCommerce.Services.NotificationAPI.Interfaces;

namespace FooCommerce.Services.NotificationAPI.Services;

public interface ITemplateService
{
    Task<INotification> GetNotificationModelAsync(NotificationPurpose purposeName, CancellationToken cancellationToken = default);
}