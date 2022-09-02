using FooCommerce.MembershipAPI.Contracts;

using MassTransit;

namespace FooCommerce.MembershipAPI.Exceptions;

public class AuthTokenStateUpdateException : Exception
{
    public Response<AuthTokenStateUpdateFailed> Response { get; }

    public AuthTokenStateUpdateException(string message, Response<AuthTokenStateUpdateFailed> response) : base(message)
    {
        Response = response;
    }
}