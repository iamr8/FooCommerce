namespace FooCommerce.MembershipService.Exceptions;

public class UserNotAuthenticatedException : Exception
{
    public UserNotAuthenticatedException() : base("Unable to find any user authenticated.")
    {
    }
}