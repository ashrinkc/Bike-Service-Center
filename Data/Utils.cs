using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace BikeServiceCenter.Data;

public static class Utils
{
    private const char _segmentDelimiter = ':';

    //method returns the path directory
    public static string GetAppDirectoryPath()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Bike-Inventory"
        );
    }
    public static string GetAppUsersFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "credentials.json");
    }

    public static string GetItemFilePath()
    {
        // combines two strings in a single path
        return Path.Combine(GetAppDirectoryPath(), "inventory.json");
    }

   
    public static string GetWithDrawItemFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "withdrawedItem.json");
    }

    // this method takes a string and returns the salted version of it.
    public static string HashSecret(string input)
    {
        var saltSize = 16;
        var iterations = 100_000;
        var keySize = 32;
        HashAlgorithmName algorithm = HashAlgorithmName.SHA256;
        byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

        return string.Join(
            _segmentDelimiter,
            Convert.ToHexString(hash),
            Convert.ToHexString(salt),
            iterations,
            algorithm
        );
    }

    // this method is used to verify hash
    public static bool VerifyHash(string input, string hashString)
    {
        string[] segments = hashString.Split(_segmentDelimiter);
        byte[] hash = Convert.FromHexString(segments[0]);
        byte[] salt = Convert.FromHexString(segments[1]);
        int iterations = int.Parse(segments[2]);
        HashAlgorithmName algorithm = new(segments[3]);
        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
            input,
            salt,
            iterations,
            algorithm,
            hash.Length
        );

        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }
}