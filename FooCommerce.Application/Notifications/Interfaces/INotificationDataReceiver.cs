using FooCommerce.Application.DbProvider;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Application.Notifications.Interfaces;

public interface INotificationDataReceiver
{
    Task ResolveReceiverInformationAsync(IDbContextFactory<AppDbContext> dbConnection, CancellationToken cancellationToken = default);
}