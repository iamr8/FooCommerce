using System.ComponentModel;

using FooCommerce.Domain.Enums;
using FooCommerce.Services.MembershipAPI.Dtos;

namespace FooCommerce.Services.MembershipAPI.Services;

public interface ICommunicationsManager
{
    Task<UserCommunicationModel> GetModelByIdAsync(Guid userCommunicationId);

    /// <summary>
    ///
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    Task<Guid?> GetVerifiedCommunicationIdAsync(CommType type, string value);
}