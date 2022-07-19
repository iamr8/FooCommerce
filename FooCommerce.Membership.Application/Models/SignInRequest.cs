namespace FooCommerce.Membership.Application.Models
{
    public sealed class SignInRequest : SignBase
    {
        public SignInRequest(string username, string password, bool isPersistent)
        {
            Username = username;
            Password = password;
            IsPersistent = isPersistent;
        }

        public bool IsPersistent { get; }
    }
}