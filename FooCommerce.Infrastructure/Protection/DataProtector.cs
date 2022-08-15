using System.Security.Cryptography;

namespace FooCommerce.Infrastructure.Protection;

public static class DataProtector
{
    public const int SaltSize = 16; // 128 bit
    public const int KeySize = 32; // 256 bit

    public static string Hash(string password, int saltSize = 16, int keySize = 32, int iterations = 10_000)
    {
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            saltSize,
            iterations,
            HashAlgorithmName.SHA256);
        var key = Convert.ToBase64String(algorithm.GetBytes(keySize));
        var salt = Convert.ToBase64String(algorithm.Salt);

        return $"{iterations}.{salt}.{key}";
    }

    public static (bool Verified, bool NeedsUpgrade) Check(string hash, string password, int keySize = 32, int iteration = 10_000)
    {
        var parts = hash.Split('.', 3);

        if (parts.Length != 3)
        {
            throw new FormatException("Unexpected hash format. " +
                                      "Should be formatted as `{iterations}.{salt}.{hash}`");
        }

        var iterations = Convert.ToInt32(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);

        var needsUpgrade = iterations != iteration;

        using var algorithm = new Rfc2898DeriveBytes(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256);
        var keyToCheck = algorithm.GetBytes(keySize);

        var verified = keyToCheck.SequenceEqual(key);

        return (verified, needsUpgrade);
    }
}