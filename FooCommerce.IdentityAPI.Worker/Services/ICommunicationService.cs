using System.ComponentModel;
using FooCommerce.Domain.Enums;
using FooCommerce.IdentityAPI.Dtos;

namespace FooCommerce.IdentityAPI.Worker.Services;

public interface ICommunicationService
{
    Task<UserCommunicationModel> GetModelByIdAsync(Guid userCommunicationId, CancellationToken cancellationToken = default);

    Task<IEnumerable<UserCommunicationModel>> GetModelsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    ///
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A nullable <see cref="Guid"/> value that represents Id of the established <see cref="UserCommunication"/>.</returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task<UserCommunicationModel> GetNonVerifiedModelByTypeAsync(CommunicationType type, string value, CancellationToken cancellationToken = default);
}