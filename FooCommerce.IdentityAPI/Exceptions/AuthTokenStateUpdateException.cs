using FooCommerce.IdentityAPI.Contracts;
using MassTransit;

namespace FooCommerce.IdentityAPI.Exceptions;

public class AuthTokenStateUpdateException : Exception
{
    public Response<AuthTokenStateUpdateFailed> Response { get; }

    public AuthTokenStateUpdateException(string message, Response<AuthTokenStateUpdateFailed> response) : base(message)
    {
        Response = response;
    }
}