using System.ComponentModel;

using FooCommerce.Domain.Enums;

namespace FooCommerce.IdentityAPI.Worker.Services;

public interface ICommunicationsManagerService
{
    //Task<UserCommunicationModel> GetModelByIdAsync(Guid userCommunicationId);

    ///// <summary>
    /////
    ///// </summary>
    ///// <param name="userId"></param>
    ///// <returns></returns>
    ///// <exception cref="ArgumentNullException"></exception>
    //Task<IEnumerable<UserCommunicationModel>> GetModelsByUserIdAsync(Guid userId);

    ///// <summary>
    /////
    ///// </summary>
    ///// <param name="type"></param>
    ///// <param name="value"></param>
    ///// <returns>A nullable <see cref="Guid"/> value that represents Id of the established <see cref="UserCommunication"/>.</returns>
    ///// <exception cref="InvalidEnumArgumentException"></exception>
    ///// <exception cref="ArgumentNullException"></exception>
    //Task<UserCommunicationModel> GetNonVerifiedModelByTypeAsync(CommunicationType type, string value);

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