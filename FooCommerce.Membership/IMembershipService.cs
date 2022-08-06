using FooCommerce.Application.DbProvider;
using FooCommerce.Membership.Entities;
using FooCommerce.Membership.Enums;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Membership
{
    public interface IMembershipService
    {
    }

    public class MembershipService : IMembershipService
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

        public MembershipService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public SignUpStatus? SignUp(SignUpRequest request)
        {
            // if (!request.Validate())
            // return null;

            return SignUpStatus.NeedEmailVerification;
        }

        public async Task<SignInStatus?> SignIn(SignInRequest request)
        {
            // if (request.Validate())
            // return null;

            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var query = from user in dbContext.Set<User>().AsNoTracking().AsSplitQuery()
                        join contact in dbContext.Set<UserContact>() on user.Id equals contact.UserId into contacts
                        join password in dbContext.Set<UserPassword>() on user.Id equals password.UserId into passwords

                        let contact = (from _contact in contacts
                                       where _contact.IsVerified && _contact.Type == UserContactType.EmailAddress && _contact.Value == request.Username.ToLowerInvariant()
                                       orderby _contact.Created
                                       select _contact).FirstOrDefault()
                        let password = passwords.OrderByDescending(c => c.Created).First()
                        select new
                        {
                            Contact = contact,
                            Password = password
                        };

            return SignInStatus.PermanentlyBanned;
        }
    }
}