using FooCommerce.Infrastructure.Exceptions;
using FooCommerce.Infrastructure.Membership.Contracts;

namespace FooCommerce.Infrastructure.Services;

public interface IMembershipService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotRegisteredException"></exception>
    Task<Guid> RegisterAsync(SignUpRequest model, CancellationToken cancellationToken = default);
}