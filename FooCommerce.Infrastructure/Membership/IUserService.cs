using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Entities.Membership;
using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Models.Membership;
using FooCommerce.Application.Services.Listings;
using FooCommerce.Application.Services.Membership;
using FooCommerce.Domain.Enums;
using FooCommerce.Domain.Services;
using FooCommerce.Infrastructure.Membership.Validators;

using MassTransit;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FooCommerce.Infrastructure.Membership;

public class UserService : IUserService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly IPasswordProtectorService _protectorService;
    private readonly ILocationService _locationService;
    private readonly ILogger<IUserService> _logger;

    public UserService(IDbContextFactory<AppDbContext> dbContextFactory, IPasswordProtectorService protectorService, ILocationService locationService, ILogger<IUserService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _locationService = locationService;
        _logger = logger;
        _protectorService = protectorService;
    }

    public async Task<bool> CheckIfEmailAlreadyEstablishedAsync(string email, DbContext dbContext, CancellationToken cancellationToken = default)
    {
        var query = from communication in dbContext.Set<UserCommunication>().AsNoTracking()
                    where communication.IsVerified &&
                          communication.Type == UserCommunicationTypes.Email &&
                          communication.Value == email.ToLowerInvariant()
                    select communication;
        var isEstablished = await query.AnyAsync(cancellationToken);
        return isEstablished;
    }

    public async Task CreateUserAsync(SignUpRequest model, DbContext dbContext, CancellationToken cancellationToken = default)
    {
        var roleId = await (from _role in dbContext.Set<Role>().AsNoTracking()
                            where _role.Type == RoleTypes.NormalUser
                            select new { _role.Id }).FirstAsync(cancellationToken: cancellationToken);

        var user = dbContext.Add(new User()).Entity;
        var password = dbContext.Add(new UserPassword(_protectorService.Hash(model.Password), user.Id)).Entity;
        var role = dbContext.Add(new UserRole(roleId.Id, user.Id)).Entity;
        var communication = dbContext.Add(new UserCommunication(UserCommunicationTypes.Email, model.Email, user.Id)).Entity;
        dbContext.AddRange(new List<UserInformation>
        {
            new(UserInformationTypes.Name, model.FirstName, user.Id),
            new(UserInformationTypes.Surname, model.LastName, user.Id)
        });
    }

    public async Task<SignUpResponse> SignUpAsync(SignUpRequest model, CancellationToken cancellationToken)
    {
        var validator = new SignUpRequestValidator(_locationService);
        var validationResult = await validator.ValidateAsync(model, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new SignUpResponse
            {
                Status = Status.InputDataNotValid,
                Errors = validationResult.Errors
            };
        }

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var isEstablished = await CheckIfEmailAlreadyEstablishedAsync(model.Email, dbContext, cancellationToken);
        if (isEstablished)
            return Status.EmailAlreadyEstablished;

        await CreateUserAsync(model, dbContext, cancellationToken);
        var saved = await dbContext.SaveChangesAsync(cancellationToken) > 0;
        if (!dbContext.ChangeTracker.HasChanges() && saved)
            return Status.Success;

        return Status.Failed;
        //var query = from user in dbContext.Set<User>().AsNoTracking()
        //    join password in dbContext.Set<UserPassword>().AsNoTracking() on user.Id equals password.UserId into
        //        passwords
        //    join communication in dbContext.Set<UserCommunication>().AsNoTracking() on user.Id equals communication
        //        .UserId into communications
        //    join lockout in dbContext.Set<UserLockout>().AsNoTracking() on user.Id equals lockout.UserId into lockouts
        //    join role in dbContext.Set<UserRole>().AsNoTracking() on user.Id equals role.UserId into roles
        //    join setting in dbContext.Set<UserSetting>().AsNoTracking() on user.Id equals setting.UserId into settings
        //    join information in dbContext.Set<UserInformation>().AsNoTracking() on user.Id equals information.UserId
        //        into informations
        //    select user;
    }
}