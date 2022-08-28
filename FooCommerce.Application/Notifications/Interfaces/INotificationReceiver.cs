using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Membership.Models;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Application.Notifications.Interfaces;

public interface INotificationReceiver
{
    Guid UserId { get; }
    string Name { get; }
    IEnumerable<UserCommunicationModel> UserCommunications { get; }

    Task ResolveInformationAsync(IDbContextFactory<AppDbContext> dbConnection, CancellationToken cancellationToken = default);
}