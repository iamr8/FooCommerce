using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Membership.Models;
using FooCommerce.Application.Notifications.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Application.Notifications.Models.Receivers;

public record NotificationReceiver : INotificationReceiver, INotificationDataReceiver
{
    public NotificationReceiver()
    {
    }

    public Guid UserId { get; set; }
    public string Name { get; set; }
    public IEnumerable<UserCommunicationModel> UserCommunications { get; set; }

    public virtual Task ResolveReceiverInformationAsync(IDbContextFactory<AppDbContext> dbConnection, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}