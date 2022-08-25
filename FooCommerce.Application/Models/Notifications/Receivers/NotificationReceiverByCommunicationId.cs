﻿using System.Data;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Entities.Membership;
using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Interfaces.Notifications;
using FooCommerce.Application.Models.Membership;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Application.Models.Notifications.Receivers;

public record NotificationReceiverByCommunicationId(Guid UserCommunicationId) : INotificationReceiver, INotificationDataReceiver
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public IEnumerable<UserCommunicationModel> UserCommunications { get; private set; }

    public async Task ResolveReceiverInformationAsync(IDbContextFactory<AppDbContext> dbConnection, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbConnection.CreateDbContextAsync(cancellationToken);

        var userId = await dbContext.Set<UserCommunication>()
            .AsNoTracking()
            .Where(x => x.Id == UserCommunicationId && x.IsVerified)
            .Select(x => new { x.UserId })
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        UserId = userId.UserId;

        var name = await dbContext.Set<UserInformation>()
            .AsNoTracking()
            .Where(x => x.UserId == UserId && x.Type == UserInformationType.Name)
            .OrderByDescending(x => x.Created)
            .Select(x => new { x.Value })
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        var communications = await dbContext.Set<UserCommunication>()
            .AsNoTracking()
            .Where(x => x.UserId == UserId && x.IsVerified && x.Id != UserCommunicationId)
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