using FooCommerce.IdentityAPI.Dtos;

namespace FooCommerce.IdentityAPI.Models;

public interface ITokenUserModel
{
    Guid UserId { get; }
    string Name { get; }

    /// <summary>
    /// A <see cref="UserCommunicationModel"/> object which is whether verified or not verified yet.
    /// </summary>
    UserCommunicationModel Communication { get; }
}