using System.Security.Cryptography;
using TestApp.Core.Interfaces;

namespace TestApp.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 10000;

    public string HashPassword(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256);
        var salt = algorithm.Salt;
        var key = algorithm.GetBytes(KeySize);

        var hashBytes = new byte[SaltSize + KeySize];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
        Buffer.BlockCopy(key, 0, hashBytes, SaltSize, KeySize);

        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var hashBytes = Convert.FromBase64String(hashedPassword);
        var salt = new byte[SaltSize];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

        using var algorithm = new Rfc2898DeriveBytes(providedPassword, salt, Iterations, HashAlgorithmName.SHA256);
        var key = algorithm.GetBytes(KeySize);

        for (int i = 0; i < KeySize; i++)
        {
            if (hashBytes[i + SaltSize] != key[i])
            {
                return false;
            }
        }

        return true;
    }
}