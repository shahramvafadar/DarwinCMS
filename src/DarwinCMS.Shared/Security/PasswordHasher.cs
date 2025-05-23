using System.Security.Cryptography;
using System.Text;

namespace DarwinCMS.Shared.Security;

/// <summary>
/// Secure password hashing using PBKDF2 with a random salt.
/// Does not store plain text or reversible encryption.
/// </summary>
public static class PasswordHasher
{
    private const int SaltSize = 16;          // 128-bit
    private const int HashSize = 32;          // 256-bit
    private const int Iterations = 100_000;   // Industry standard (high but performant)

    /// <summary>
    /// Hashes a password using PBKDF2 + random salt.
    /// The final format is: [Base64(salt)]:[Base64(hash)]
    /// </summary>
    public static string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.", nameof(password));

        // Generate salt
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        // Generate hash
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password: password,
            salt: salt,
            iterations: Iterations,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: HashSize);

        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    /// <summary>
    /// Verifies if the given password matches the stored hash.
    /// </summary>
    public static bool Verify(string password, string storedHash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedHash))
            return false;

        var parts = storedHash.Split(':');
        if (parts.Length != 2)
            return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] expectedHash = Convert.FromBase64String(parts[1]);

        byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
            password: password,
            salt: salt,
            iterations: Iterations,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: HashSize);

        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}
