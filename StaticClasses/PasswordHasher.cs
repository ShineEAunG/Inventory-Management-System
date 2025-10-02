using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace InventoryManagementSystem.StaticClasses;
public static class PasswordHasher
{
    private static (byte[] randomBytes, byte[] hashedBytes) PasswordBasedKeyDerivationFun2(string password, byte[]? randomBytes)
    {
        var size = 128 / 8; // 16 bytes
        if (randomBytes is null)
        {
            randomBytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);  // get randomized bytes and assign to randomBytes
            }
        }
        var psuedoRandomFunction = KeyDerivationPrf.HMACSHA256; // standard algorithm
        var iterationCount = 100000;    // owaps recommanded ammount
        var requestedBytes = 256 / 8;   // 32 bytes required for hmacsha256
        var PasswordBasedKeyDerivation = KeyDerivation.Pbkdf2(password, randomBytes, psuedoRandomFunction, iterationCount, requestedBytes);
        return (randomBytes, PasswordBasedKeyDerivation);
    }
    
    public static string HashPassword(string password)  // hash Password = Base64(RandomBytes).Base64(keyderivationfun2)
    {
        var (randomBytes, hashedBytes) = PasswordBasedKeyDerivationFun2(password, null);
        var hashedPassword = $"{Convert.ToBase64String(randomBytes)}.{Convert.ToBase64String(hashedBytes)}";
        return hashedPassword;
    }
    public static bool VerifyPassword(string rawPassword, string hashedInDb)    // when varifying random64 => randomBytes => hash raw using randomBytes in Db 
    {                                                                           // => check if they are equivalent 
        var split = hashedInDb.Split('.');
        if (split.Length != 2)
            return false;
        var random64 = split[0];
        var hashed64InDb = split[1];
        var hashedBytesInDb = Convert.FromBase64String(hashed64InDb);
        byte[] randomBytes = Convert.FromBase64String(random64);
        var Pbkdf2 = PasswordBasedKeyDerivationFun2(rawPassword, randomBytes);
        var hashedBytesOfRawPassword = Pbkdf2.Item2;
        // return Convert.ToBase64String(hashedBytesInDb) == split[1]; // string camparison
        return CryptographicOperations.FixedTimeEquals(hashedBytesOfRawPassword, hashedBytesInDb);  // safer than the string comparison        
    }
}