using System.Security.Cryptography;

namespace Domain.ValueObjects;

public class Password : ValueObject
{
    private const string ValidCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    private const string SpecialCharacters = "!@#$%ˆ&*(){}[];çÇ";

    public string Hash { get; } = string.Empty;
    public string ResetCode { get; } = Guid.NewGuid().ToString("N")[0..8];

    protected Password() { }
    
    public Password(string? password)
    {
        if  (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            password = GenerateRandomPassword();

        Hash = HashPassword(password);
    }

    private static string GenerateRandomPassword(
        short length = 16,
        bool includeSpecialCharacters = true,
        bool upperCase = false)
    {
        var chars = includeSpecialCharacters ? (ValidCharacters + SpecialCharacters) : ValidCharacters;
        var startRandom = upperCase ? 26 : 0;
        var index = 0;
        var passwordChars = new char[length];
        var randomGenerator = new Random();

        while (index < length)
        {
            passwordChars[index++] = chars[randomGenerator.Next(startRandom, chars.Length)];
        }
        
        return new string(passwordChars);
    }

    private static string HashPassword(
        string password,
        short saltSize = 16,
        short keySize = 32,
        int iteractions = 1000,
        char splitChar = '.')
    {
        if (string.IsNullOrEmpty(password))
            throw new Exception("Password can't be empty or null");

        password += Configuration.Secrets.PasswordSaltKey;

        using var hashingAlgorithm = new Rfc2898DeriveBytes(password, saltSize, iteractions, HashAlgorithmName.SHA256);
        
        var key = Convert.ToBase64String(hashingAlgorithm.GetBytes(keySize));
        var salt = Convert.ToBase64String(hashingAlgorithm.Salt);

        return $"{iteractions}{splitChar}{salt}{splitChar}{key}";
    }

    public static bool Verify(
        string hash,
        string password,
        short keySize = 32,
        int iteractions = 1000,
        char splitChar = '.')
    {
        password += Configuration.Secrets.PasswordSaltKey;
        
        var hashParts = hash.Split(splitChar, 3);

        if (hashParts.Length != 3)
            return false;
        
        var hashIterations = Convert.ToInt32(hashParts[0]);
        var salt = Convert.FromBase64String(hashParts[1]);
        var key = Convert.FromBase64String(hashParts[2]);

        if (hashIterations != iteractions)
            return false;
        
        using var hashingAlgorithm = new Rfc2898DeriveBytes(password, salt, iteractions, HashAlgorithmName.SHA256);
        
        var keyToCheck = hashingAlgorithm.GetBytes(keySize);
        
        return keyToCheck.SequenceEqual(key);
    }
}