using FooCommerce.Application.Membership.Entities;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Core.DbProvider.DbContextProvider;
using FooCommerce.Core.Membership.Models;
using FooCommerce.Core.Notifications.Enums;
using FooCommerce.Core.Notifications.Interfaces;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

// ReSharper disable MemberCanBePrivate.Global

namespace FooCommerce.Core.Notifications.Models;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public record NotificationReceiverProvider : INotificationReceiver
{
    internal NotificationReceiverProvider()
    {
    }

    public NotificationReceiverProvider(NotificationReceiverStrategy strategy, [NotNull] params object[] args)
    {
        Strategy = strategy;
        Args = args;
    }

    public Guid UserId { get; set; }
    public string Name { get; set; }
    public IEnumerable<UserCommunicationModel> UserCommunications { get; set; }
    public NotificationReceiverStrategy Strategy { get; }
    public object[] Args { get; }

    public virtual async Task ResolveInformationAsync(IDbContextFactory<AppDbContext> dbConnection, CancellationToken cancellationToken = default)
    {
        if (Args == null || !Args.Any())
            throw new NullReferenceException($"{nameof(Args)} is expected to have at least one item.");

        await using var dbContext = await dbConnection.CreateDbContextAsync(cancellationToken);

        if (Strategy == NotificationReceiverStrategy.ByUserCommunicationId)
        {
            var _userCommunicationId = Guid.Parse(Args[0].ToString());
            var userId = await dbContext.Set<UserCommunication>()
                .AsNoTracking()
                .Where(x => x.Id == _userCommunicationId && x.IsVerified)
                .Select(x => new { x.UserId })
                .FirstAsync(cancellationToken: cancellationToken);
            UserId = userId.UserId;
        }
        else
        {
            UserId = Guid.Parse(Args[0].ToString());
        }

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
        if (communications == null || !communications.Any())
            throw new NullReferenceException("Unable to find corresponding communications.");

        foreach (var com in communications)
        {
            UserCommunications ??= new List<UserCommunicationModel>();
            UserCommunications = UserCommunications.Concat(new[]
            {
                new UserCommunicationModel(com.Id, com.Type, com.Value)
            });
        }

        var name = await dbContext.Set<UserInformation>()
            .AsNoTracking()
            .Where(x => x.UserId == UserId && x.Type == UserInformationType.Name)
            .OrderByDescending(x => x.Created)
            .Select(x => new { x.Value })
            .FirstAsync(cancellationToken: cancellationToken);
        Name = name!.Value;
    }
}