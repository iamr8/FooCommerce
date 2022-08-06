namespace FooCommerce.Domain.Services;

public interface IPasswordProtectorService
{
    string Hash(string password);

    (bool Verified, bool NeedsUpgrade) Check(string hash, string password);
}