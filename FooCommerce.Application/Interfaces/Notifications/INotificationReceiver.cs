using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Models.Membership;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Application.Interfaces.Notifications;

public interface INotificationReceiver
{
    Guid UserId { get; }
    string Name { get; }
    List<UserCommunicationModel> UserCommunications { get; }

    Task FetchAsync(IDbContextFactory<AppDbContext> dbConnection, CancellationToken cancellationToken = default);
}