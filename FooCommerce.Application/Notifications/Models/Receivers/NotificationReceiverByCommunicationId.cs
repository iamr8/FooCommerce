using System.Data;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Membership.Entities;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Membership.Models;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Application.Notifications.Models.Receivers;

public record NotificationReceiverByCommunicationId : NotificationReceiver
{
    private readonly Guid _userCommunicationId;

    public NotificationReceiverByCommunicationId(Guid userCommunicationId) : base()
    {
        _userCommunicationId = userCommunicationId;
    }

    public override async Task ResolveReceiverInformationAsync(IDbContextFactory<AppDbContext> dbConnection, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbConnection.CreateDbContextAsync(cancellationToken);

        var userId = await dbContext.Set<UserCommunication>()
            .AsNoTracking()
            .Where(x => x.Id == _userCommunicationId && x.IsVerified)
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
            .Where(x => x.UserId == UserId && x.IsVerified && x.Id != _userCommunicationId)
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