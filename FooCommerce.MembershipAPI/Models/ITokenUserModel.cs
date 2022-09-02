using FooCommerce.MembershipAPI.Dtos;

namespace FooCommerce.MembershipAPI.Models;

public interface ITokenUserModel
{
    Guid UserId { get; }
    string Name { get; }

    /// <summary>
    /// A <see cref="UserCommunicationModel"/> object which is whether verified or not verified yet.
    /// </summary>
    UserCommunicationModel Communication { get; }
}