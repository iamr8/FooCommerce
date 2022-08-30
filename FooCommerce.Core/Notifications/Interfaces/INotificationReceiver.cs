using FooCommerce.Core.DbProvider.DbContextProvider;
using FooCommerce.Core.Membership.Models;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Core.Notifications.Interfaces;

public interface INotificationReceiver
{
    Guid UserId { get; }
    string Name { get; }
    IEnumerable<UserCommunicationModel> UserCommunications { get; }

    Task ResolveInformationAsync(IDbContextFactory<AppDbContext> dbConnection, CancellationToken cancellationToken = default);
}