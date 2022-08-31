using FooCommerce.Application.Communications.Models;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.NotificationAPI.Interfaces;

public interface INotificationReceiver
{
    Guid UserId { get; }
    string Name { get; }
    IEnumerable<UserCommunicationModel> UserCommunications { get; }

    Task ResolveInformationAsync<TDbContext>(IDbContextFactory<TDbContext> dbConnection, CancellationToken cancellationToken = default) where TDbContext : DbContext;
}