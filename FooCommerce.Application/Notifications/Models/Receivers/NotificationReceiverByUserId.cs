using System.Data;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Membership.Entities;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Membership.Models;
using FooCommerce.Application.Notifications.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Application.Notifications.Models.Receivers;

public record NotificationReceiverByUserId(Guid UserId) : INotificationReceiver, INotificationDataReceiver
{
    public string Name { get; private set; }

    public IEnumerable<UserCommunicationModel> UserCommunications { get; private set; }

    public async Task ResolveReceiverInformationAsync(IDbContextFactory<AppDbContext> dbConnection, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbConnection.CreateDbContextAsync(cancellationToken);
        var name = await dbContext.Set<UserInformation>()
            .AsNoTracking()
            .Where(x => x.UserId == UserId && x.Type == UserInformationType.Name)
            .OrderByDescending(x => x.Created)
            .Select(x => new { x.Value })
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        var communications = await dbContext.Set<UserCommunication>()
            .AsNoTracking()
            .Where(x => x.UserId == UserId && x.IsVerified)
            .Select(x => new
            {
                x.Id,
                x.Type,
                x.Value,
            })
            .ToListAsync(cancellationToken: cancellationToken);

        Name = name!.Value;
        communications.ForEach(com => UserCommunications = UserCommunications.Concat(new[]{new UserCommunicationModel
        {
            Id = com.Id,
            Value = com.Value,
            Type = com.Type
        }}));
    }
}