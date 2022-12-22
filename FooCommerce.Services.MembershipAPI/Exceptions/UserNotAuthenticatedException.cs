namespace FooCommerce.Services.MembershipAPI.Exceptions;

public class UserNotAuthenticatedException : Exception
{
    public UserNotAuthenticatedException() : base("Unable to find any user authenticated.")
    {
    }
}