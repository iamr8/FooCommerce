using FooCommerce.Core.DbProvider;
using FooCommerce.Core.Membership.Models;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.NotificationAPI.Bridge.Interfaces;

public interface INotificationReceiver
{
    Guid UserId { get; }
    string Name { get; }
    IEnumerable<UserCommunicationModel> UserCommunications { get; }

    Task ResolveInformationAsync(IDbContextFactory<AppDbContext> dbConnection, CancellationToken cancellationToken = default);
}