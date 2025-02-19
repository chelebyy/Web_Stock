using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace StockAPI.Services;

public class HashService : IHashService
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 10000;
    private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
    private static readonly char Delimiter = ';';
    private const string Salt = "StockAPI_Salt_2024";

    public string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));

        using var sha256 = SHA256.Create();
        var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hashedBytes = sha256.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hashedBytes);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));

        if (string.IsNullOrEmpty(hashedPassword))
            throw new ArgumentNullException(nameof(hashedPassword));

        var newHashedPassword = HashPassword(password);
        return hashedPassword == newHashedPassword;
    }

    public static string ComputeHash(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            input,
            salt,
            Iterations,
            _hashAlgorithmName,
            KeySize);

        return string.Join(
            Delimiter,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash));
    }

    public static bool VerifyHash(string input, string hashString)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(hashString);

        string[] segments = hashString.Split(Delimiter);
        if (segments.Length != 2)
        {
            return false;
        }

        byte[] salt = Convert.FromBase64String(segments[0]);
        byte[] hash = Convert.FromBase64String(segments[1]);

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
            input,
            salt,
            Iterations,
            _hashAlgorithmName,
            KeySize);

        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }
} 