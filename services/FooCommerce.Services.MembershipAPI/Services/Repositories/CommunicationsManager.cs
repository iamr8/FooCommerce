using System.ComponentModel;
using System.Data;

using FooCommerce.Domain.Enums;
using FooCommerce.Services.MembershipAPI.DbProvider;
using FooCommerce.Services.MembershipAPI.Dtos;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Services.MembershipAPI.Services.Repositories;

public class CommunicationsManager : ICommunicationsManager
{
    private readonly IDbContextFactory<MembershipDbContext> _dbContextFactory;

    public CommunicationsManager(IDbContextFactory<MembershipDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    #region Statics

    /// <summary>
    ///
    /// </summary>
    /// <param name="userCommunicationId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<UserCommunicationModel> GetModelByIdAsync(Guid userCommunicationId)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var communication = await dbContext.UserCommunications
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Id == userCommunicationId)
            .Select(x => new UserCommunicationModel
            {
                Type = x.Type,
                Value = x.Value,
                Id = x.Id,
                IsVerified = x.IsVerified
            })
            .SingleOrDefaultAsync();
        return communication;
    }

    #endregion Statics

    #region Publics

    public async Task<Guid?> GetVerifiedCommunicationIdAsync(CommType type, string value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        if (!Enum.IsDefined(typeof(CommType), type))
            throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(CommType));

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var communication = await dbContext.UserCommunications
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Type == type && x.Value == value && x.IsVerified)
            .Select(x => new
            {
                Id = x.Id,
            })
            .SingleOrDefaultAsync();
        return communication?.Id;
    }

    #endregion Publics
}